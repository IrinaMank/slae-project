using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using slae_project.Vector;

namespace slae_project.Matrix
{
    public class CoordinateMatrix : IMatrix
    {
        /// <summary>
        /// Класс для удобного и быстрого доступа к элементам транспонированной матрицы
        /// без создания таковой
        /// </summary>
        public class TransposeIllusion : ILinearOperator
        {
            public CoordinateMatrix Matrix { get; set; }
            public ILinearOperator Transpose => Matrix;
            public ILinearOperator T => Matrix;
            public IVector Diagonal => Matrix.Diagonal;
            public int Size => Matrix.Size;
            public IVector MultL(IVector x, bool UseDiagonal) => Matrix.MultLT(x, UseDiagonal);
            public IVector SolveL(IVector x, bool UseDiagonal) => Matrix.SolveLT(x, UseDiagonal);
            public IVector Mult(IVector x) => Matrix.MultT(x);
            public IVector MultU(IVector x, bool UseDiagonal) => Matrix.MultUT(x, UseDiagonal);
            public IVector SolveU(IVector x, bool UseDiagonal) => Matrix.SolveUT(x, UseDiagonal);
        }
        // Элементы матрицы
        Dictionary<(int i, int j), double> elements = new Dictionary<(int i, int j), double>();
        //Идентефикатор выполненности LU - разложения
        // = false после любого изменения матрицы
        // = true после выполнения LU - разложения
        bool LU_was_made = false;
        private List<double[]> L;
        private List<double[]> U;
        public double this[int i, int j]
        {

            get
            {
                try
                {
                    return elements[(i, j)];
                }
                catch (KeyNotFoundException ex)
                {
                    return 0;
                }
            }
            set
            {
                if (value != 0)
                {
                    elements[(i, j)] = value;
                    // Это нормально, с учетом того, что матрицы не часто меняют
                    LU_was_made = false;
                }
            }
        }

        // Предполагаются только квадратные матрицы
        public int Size { get; }
        public ILinearOperator Transpose => new TransposeIllusion { Matrix = this };
        public ILinearOperator T => new TransposeIllusion { Matrix = this };

        //TODO: Метод и правда должен что-то возвращать
        public IVector Diagonal => throw new NotImplementedException();

        // Для выпендрежников, которые решили обойти матрицу поэлементно
        public IEnumerator<(double value, int row, int col)> GetEnumerator()
        {
            IEnumerable<(double value, int row, int col)> F()
            {
                foreach (var p in elements)
                    yield return (p.Value, p.Key.i, p.Key.j);
            }
            return F().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /// <summary>
        /// Инициализация матрицы массивом координат и массивом значений
        /// </summary>
        /// <param name="coord">Массив координат размерности (N,2)</param>
        /// <param name="val">Массив значений</param>
        public CoordinateMatrix(int[][] coord, double[] val)
        {
            if (coord.Length != val.Length)
                MessageBox.Show("Размер массива координат не совпадает с размером массива значений. Выбран наименьший.",
                      "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            int maxij = 0;
            for (int i = 0; i < val.Length; i++)
            {
                this.elements.Add((coord[i][0], coord[i][1]), val[i]);
                if (coord[i][0] > maxij)
                    maxij = coord[i][0];
                if (coord[i][1] > maxij)
                    maxij = coord[i][1];
            }
            this.Size = maxij+1;
        }
        /// <summary>
        /// Инициализация матрицы массивом координат и массивом значений
        /// </summary>
        /// <param name="coord">Массив координат размерности (N,2)</param>
        /// <param name="val">Массив значений</param>
        public CoordinateMatrix((int x, int y)[] coord, double[] val)
        {
            if (coord.Length != val.Length)
                MessageBox.Show("Размер массива координат не совпадает с размером массива значений. Выбран наименьший.",
                      "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            int maxij = 0;
            for (int i = 0; i < val.Length; i++)
            {
                this.elements.Add(coord[i], val[i]);
                if (coord[i].x > maxij)
                    maxij = coord[i].x;
                if (coord[i].x > maxij)
                    maxij = coord[i].y;
            }
            this.Size = maxij+1;
        }
        public IVector Mult(IVector x)
        {
            if (this.Size != x.Size)
            {
                MessageBox.Show("Попытка умножения матрицы и вектора не соответствующих размерностей. Метод 'Mult' вернул null. В следующий раз будь аккуратнее :3",
                      "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            IVector result = new SimpleVector(Size);
            foreach (var el in elements)
            {
                result[el.Key.i] += el.Value * x[el.Key.j];
            }
            return result;
        }
        //TODO: Написать эффективный алгоритм
        //С учетом того, что портрет не сохраняется
        private void MakeLU()
        { //Выделение памяти
            L = new List<double[]> { };
            U = new List<double[]> { };
            for (int i = 1; i <= Size; i++)
            {
                L.Add(new double[i]);
                U.Add(new double[Size - i + 1]);
            }
            // Разложение
            try
            {
                for (int i = 0; i < Size; i++)
                {
                    L[i][0] = this[i, 0];
                    U[0][i] = this[0, i] / L[0][0];
                }
                double sum;
                for (int i = 1; i < Size; i++)
                {
                    for (int j = 1; j < Size; j++)
                    {
                        if (i >= j)
                        {
                            sum = 0;
                            for (int k = 0; k < j; k++)
                                sum += L[i][k] * U[k][j-k];

                            L[i][j] = this[i, j] - sum;
                        }

                        if (j >= i)
                        {
                            sum = 0;
                            for (int k = 0; k < i; k++)
                                sum += L[i][k] * U[k][j-k];

                            U[i][j-i] = (this[i, j] - sum) / L[i][i];
                        }
                    }
                }
                LU_was_made = true;
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Не удалось выполнить LU-разложение матрицы. Мы обязательно постараемся с этим что-нибудь сделать в будущем, а пока ловите лучи счастья: ========",
                      "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LU_was_made = false;
            }
        }
        public IVector SolveL(IVector x, bool UseDiagonal = true)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                int d;
                if (UseDiagonal)
                    d = 0;
                else
                    d = 1;

                IVector result = new SimpleVector(Size);
                if (!UseDiagonal)
                {
                    if (Math.Abs(x[0]) < 1e-9)
                    {
                        result[0] = 0;
                    }
                    else
                        MessageBox.Show("Система неразрешима. Попробуйте решить с использованием диагонали. Метод 'SolveL' вернул null.",
                              "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                for (int i = 0 + d; i < L.Count; i++)
                {
                    var line = L[i];
                    result[i] = x[i];
                    for (int j = 0; j < line.Length - 1 - d; j++)
                        result[i] -= result[j] * line[j];
                    try
                    {
                        result[i] /= line[line.Length - 1 - d];
                    }
                    catch (DivideByZeroException)
                    {
                        MessageBox.Show("Метод 'SolveL' выполнить не удалось, так как в ходе решения СЛАУ произошло деление на ноль. Будем работать над тем, чтобы такого не происходило. Метод вернул null.",
                              "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
                return result;
            }
            MessageBox.Show("Метод 'SolveL' выполнить не удалось, так как не удалось разложить матрицу. Метод вернул null.",
                  "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        //Метод еще не готов
        public IVector SolveU(IVector x, bool UseDiagonal = true)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                int d;
                if (UseDiagonal)
                    d = 0;
                else
                    d = 1;

                IVector result = new SimpleVector(Size);
                if (!UseDiagonal)
                {
                    if (Math.Abs(x[Size - 1]) < 1e-13)
                    {
                        result[Size - 1] = 0;
                    }
                    else
                        MessageBox.Show("Система неразрешима. Попробуйте решить с использованием диагонали. Метод 'SolveU' вернул null.",
                              "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                for (int i = Size - 1 - d; i >= 0; i--)
                {
                    var line = U[i];
                    var offset = Size - line.Length;
                    result[i] = x[i];
                    for (int j = 1 + d; j < line.Length; j++)
                        result[i] -= result[j + offset] * line[j];
                    try
                    {
                        result[i] /= line[0 + d];
                    }
                    catch (DivideByZeroException)
                    {
                        MessageBox.Show("Метод 'SolveU' выполнить не удалось, так как в ходе решения СЛАУ произошло деление на ноль. Будем работать над тем, чтобы такого не происходило. Метод вернул null.",
                              "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
                return result;
            }
            MessageBox.Show("Метод 'SolveU' выполнить не удалось, так как не удалось разложить матрицу. Метод вернул null.",
                  "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        /// <summary>
        /// Обобщение уможения на матрицы разложения
        /// </summary>
        /// <param name="x"></param>
        /// <param name="partM">Либо L-, либо U-матрица из данного класса</param>
        /// <param name="use_diagonal"></param>
        /// <returns></returns>
        private IVector CommoLUMult(IVector x, List<double[]> partM, bool use_diagonal, bool transpose)
        {
            int end, t;

            if (use_diagonal)
                end = 0;
            else
                end = 1;

            IVector result = new SimpleVector(Size);
            if (transpose)
            {
                for (int i = 0; i < partM.Count; i++)
                {
                    var line = partM[i];
                    for (int j = 0; j < line.Length - end; j++)
                    {
                        result[i] += line[j] * x[j];
                    }
                }
            }
            else
            {
                for (int i = 0; i < partM.Count; i++)
                {
                    var line = partM[i];
                    for (int j = 0; j < line.Length - end; j++)
                    {
                        result[j] += line[j] * x[j];
                    }
                }
            }
            return result;
        }
        public IVector MultL(IVector x, bool UseDiagonal = true)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
                return CommoLUMult(x, L, UseDiagonal, false);
            MessageBox.Show("Метод 'MultL' выполнить не удалось, так как не удалось разложить матрицу. Метод вернул null.",
                  "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        public IVector MultU(IVector x, bool UseDiagonal = true)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
                return CommoLUMult(x, U, UseDiagonal, false);
            MessageBox.Show("Метод 'MultU' выполнить не удалось, так как не удалось разложить матрицу. Метод вернул null.",
                  "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        protected IVector MultT(IVector x)
        {
            if (this.Size != x.Size)
            {
                MessageBox.Show("Попытка умножения матрицы и вектора не соответствующих размерностей. Метод 'Mult' вернул null. В следующий раз будь аккуратнее :3",
                      "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            IVector result = new SimpleVector(Size);
            foreach (var el in elements)
            {
                result[el.Key.j] += el.Value * x[el.Key.i];
            }
            return result;
        }
        //Уверен все можно свести к одному методу
        //Но зачем, когда и так неплохо работает?
        protected IVector SolveLT(IVector x, bool UseDiagonal = true)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                int d;
                if (UseDiagonal)
                    d = 0;
                else
                    d = 1;

                IVector result = new SimpleVector(Size);
                for (int i = 0; i < Size; i++)
                    result[i] = x[i];

                if (!UseDiagonal)
                {
                    if (Math.Abs(x[Size - 1]) < 1e-9)
                    {
                        result[Size-1] = 0;
                    }
                    else
                        MessageBox.Show("Система неразрешима. Попробуйте решить с использованием диагонали. Метод 'SolveL' вернул null.",
                              "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                for (int i = Size - 1; i >= d; i--)
                {
                    var line = L[i];
                    var offset = Size - line.Length;
                    for (int j = 0; j < line.Length - d; j++)
                    {
                        result[j] -= result[i] * L[i][j];
                    }
                    try
                    {
                        if (UseDiagonal)
                        {
                            result[i] /= line[line.Length - 1];
                        }
                        else
                        {
                            result[i] /= L[i + 1][line.Length - 2];
                        }
                    }
                    catch (DivideByZeroException)
                    {
                        MessageBox.Show("Метод 'SolveL' выполнить не удалось, так как в ходе решения СЛАУ произошло деление на ноль. Будем работать над тем, чтобы такого не происходило. Метод вернул null.",
                              "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
            }
            MessageBox.Show("Метод 'SolveL' выполнить не удалось, так как не удалось разложить матрицу. Метод вернул null.",
                  "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        protected IVector SolveUT(IVector x, bool UseDiagonal = true)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                int d;
                if (UseDiagonal)
                    d = 0;
                else
                    d = 1;

                IVector result = new SimpleVector(Size);
                for (int i = 0; i < Size; i++)
                    result[i] = x[i];

                if (!UseDiagonal)
                {
                    if (Math.Abs(x[0]) < 1e-9)
                    {
                        result[0] = 0;
                    }
                    else
                        MessageBox.Show("Система неразрешима. Попробуйте решить с использованием диагонали. Метод 'SolveL' вернул null.",
                              "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                for (int i = 0; i < Size - d; i++)
                {
                    var line = U[i];
                    for (int j = 1 + d; j < line.Length; j++)
                    {
                        result[j] -= result[i] * U[i][j];
                    }
                    try
                    {
                        if (UseDiagonal)
                        {
                            result[i] /= line[0];
                        }
                        else
                        {
                            result[i] /= U[i - 1][1];
                        }
                    }
                    catch (DivideByZeroException)
                    {
                        MessageBox.Show("Метод 'SolveL' выполнить не удалось, так как в ходе решения СЛАУ произошло деление на ноль. Будем работать над тем, чтобы такого не происходило. Метод вернул null.",
                              "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
            }
            MessageBox.Show("Метод 'SolveL' выполнить не удалось, так как не удалось разложить матрицу. Метод вернул null.",
                  "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        protected IVector MultLT(IVector x, bool UseDiagonal = true)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
                return CommoLUMult(x, L, UseDiagonal, true);
            MessageBox.Show("Метод 'MultL' выполнить не удалось, так как не удалось разложить матрицу. Метод вернул null.",
                  "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        protected IVector MultUT(IVector x, bool UseDiagonal = true)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
                return CommoLUMult(x, U, UseDiagonal, true);
            MessageBox.Show("Метод 'MultU' выполнить не удалось, так как не удалось разложить матрицу. Метод вернул null.",
                  "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
    }
}