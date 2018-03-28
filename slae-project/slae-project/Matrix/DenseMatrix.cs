using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using slae_project.Vector;
using slae_project.Matrix.MatrixExceptions;
using slae_project.Preconditioner;
using System.IO;

namespace slae_project.Matrix
{
    public class DenseMatrix : IMatrix
    {
        //Описание необходимых для работы файлов
        public static Dictionary<string, string> requiredFileNames => new Dictionary<string, string>
        {
            {
                "dense",
                "Файл должен содержать в первой строке размер матрицы, "+
                "в последующих строках должны быть элементы строки разделенных пробелом."
            },
        };
        public class TransposeIllusion : ILinearOperator
        {
            public DenseMatrix Matrix { get; set; }
            public ILinearOperator Transpose => Matrix;
            public ILinearOperator T => Matrix;
            public IVector Diagonal => Matrix.Diagonal;
            public int Size => Matrix.Size;
            public IVector MultL(IVector x, bool UseDiagonal) => Matrix.MultLT(x, UseDiagonal);
            public IVector SolveL(IVector x, bool UseDiagonal) => Matrix.SolveLT(x, UseDiagonal);
            public IVector Mult(IVector x, bool UseDiagonal) => Matrix.MultT(x, UseDiagonal);
            public IVector MultU(IVector x, bool UseDiagonal) => Matrix.MultUT(x, UseDiagonal);
            public IVector SolveU(IVector x, bool UseDiagonal) => Matrix.SolveUT(x, UseDiagonal);
            public IVector SolveD(IVector x) => Matrix.SolveD(x);
            public void MakeLU() => throw new NotImplementedException();
            public object Clone() => Matrix.Clone();

            public IVector MultD(IVector a)
            {
                throw new NotImplementedException();
            }
        }
        // Матрица
        private double[,] d_matrix;
        //Переменная, необходимая для реализации возможности наличия в матрицы двух диагоналей ( например в случае LU - разложенной матрицы)
        // Если extraDiagVal = 0, то считается, что в матрице одна диагональ
        // Если extraDiagVal != 0, то считается, что нижний треугольник матрицы содержит диагональ, заполненную значениями extraDiagVal
        double extraDiagVal = 0;
        bool isSymmetric = false;
        // Значение, начиная с которого любое число считается равным нулю
        private double EQU_TO_ZERO { get; } = 1e-10;
        public double this[int i, int j]
        {
            get
            {
                try
                {
                    return d_matrix[i, j];
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
                    d_matrix[i, j] = value;
                }
            }
        }
        public int Size { get; private set; }
        public ILinearOperator Transpose => new TransposeIllusion { Matrix = this };
        public ILinearOperator T => new TransposeIllusion { Matrix = this };
        public IVector Diagonal
        {
            get
            {
                IVector diag = new SimpleVector(Size);
                for (int i = 0; i < Size; i++)
                    diag[i] = d_matrix[i, i];
                return diag;
            }
        }
        // Для выпендрежников, которые решили обойти матрицу поэлементно
        public IEnumerator<(double value, int row, int col)> GetEnumerator()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    yield return (d_matrix[i, j], i, j);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /// <summary>
        /// Инициализация матрицы массивом значений
        /// </summary>
        /// <param name="val">Двумерный массив значений</param>
        public DenseMatrix(double[,] val, bool isSymmetric = false)
        {
            this.isSymmetric = isSymmetric;
            this.Size = val.GetLength(0);
            d_matrix = new double[Size, Size];
            for (int i = 0; i < val.GetLength(0); i++)
            {
                for (int j = 0; j < val.GetLength(0); j++)
                {
                    this.d_matrix[i, j] = val[i, j];
                }
            }

        }

        public DenseMatrix(int Size, bool isSymmetric = false)
        {
            this.isSymmetric = isSymmetric;
            d_matrix = new double[Size, Size];
            this.Size = Size;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    this.d_matrix[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// Инициализация матрицы координатной матрицей
        /// </summary>
        /// <param name="c_matrix">Матрица в координатном формате</param>
        public DenseMatrix(CoordinateMatrix c_matrix)
        {
            d_matrix = new double[c_matrix.Size, c_matrix.Size];
            this.Size = Size;

            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    d_matrix[i, j] = 0;

            foreach (var val in c_matrix)
                this.d_matrix[val.row, val.col] = val.value;
        }

        public DenseMatrix(bool isSymmetric = false)
        {
            this.isSymmetric = isSymmetric;
            this.Size = 0;
        }

        public IVector Mult(IVector x, bool UseDiagonal = true)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");

            IVector result = new SimpleVector(Size);
            if (UseDiagonal)
            {
                    for (int i = 0; i < Size; i++)
                        for (int j = 0; j < Size; j++)
                            result[i] += d_matrix[i, j] * x[j];
            }
            else
            {
                    for (int i = 0; i < Size; i++)
                        for (int j = 0; j < Size; j++)
                            if (i != j)
                                result[i] += d_matrix[i, j] * x[j];
            }
            return result;
        }

        /// <summary>
        /// LU разложение
        /// </summary>
        public void MakeLU()
        {
            try
            {

                for (int k = 0; k < Size; k++)
                {
                    for (int j = k; j < Size; j++)
                    {
                        double sum = 0;
                        for (int u = 0; u < k; u++)
                            sum += this[k, u] * this[u, j];
                        this[k, j] = this[k, j] - sum;
                    }
                    for (int i = k + 1; i < Size; i++)
                    {
                        double sum = 0;
                        for (int u = 0; u < k; u++)
                            sum += this[i, u] * this[u, k];
                        if (this[k, k] == 0)
                            throw new DivideByZeroException();
                        this[i, k] = (this[i, k] - sum) / this[k, k];
                    }
                }
                extraDiagVal = 1;
            }
            catch (DivideByZeroException)
            {
                throw new LUFailException("Произошло деление на ноль.");
            }
        }
        public IVector SolveL(IVector x, bool UseDiagonal = true)
        {
            IVector result = new SimpleVector(Size);
            if (!UseDiagonal)
            {
                if (Math.Abs(x[0]) < EQU_TO_ZERO)
                {
                    result[0] = 0;
                }
                else
                    throw new SlaeNotCompatipableException("Система неразрешима.");
            }
            for (int i = 0; i < Size; i++)
            {
                result[i] = x[i];
                for (int j = 0; j < i; j++)
                    result[i] -= result[j] * this[i, j];
                try
                {
                    if (extraDiagVal == 0)
                        result[i] /= this[i, i];
                    else
                        result[i] /= extraDiagVal;
                }
                catch (DivideByZeroException)
                {
                    throw new SlaeNotCompatipableException("Произошло деление на ноль.");
                }
            }
            return result;
        }

        public IVector SolveU(IVector x, bool UseDiagonal = true)
        {
            IVector result = new SimpleVector(Size);
            if (!UseDiagonal)
            {
                if (Math.Abs(x[Size - 1]) < EQU_TO_ZERO)
                {
                    result[Size - 1] = 0;
                }
                else
                    throw new SlaeNotCompatipableException("Система неразрешима.");
            }
            for (int i = Size - 1; i >= 0; i--)
            {
                int line_length = Size - i;
                result[i] = x[i];
                for (int j = i + 1; j < Size; j++)
                    result[i] -= result[j] * this[i, j];
                try
                {
                    result[i] /= this[i, i];
                }
                catch (DivideByZeroException)
                {
                    throw new SlaeNotCompatipableException("Произошло деление на ноль.");
                }
            }
            return result;
        }
        public IVector MultL(IVector x, bool UseDiagonal = true)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");

            IVector result = new SimpleVector(Size);
            if (UseDiagonal)
            {
                if (extraDiagVal == 0)
                {
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                        {
                            //нижний треугольник с диагональю
                            if (i >= j)
                            {

                                result[i] += d_matrix[i, j] * x[j];
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                        {
                            if (i > j)
                            {
                                result[i] += d_matrix[i, j] * x[j];
                                continue;
                            }
                            //диагональ
                            if (i == j)
                            {
                                result[i] += extraDiagVal * x[j];
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        if (i > j)
                            result[i] += d_matrix[i, j] * x[j];
                    }
                }
            }
            return result;
        }

        public IVector MultU(IVector x, bool UseDiagonal = true)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");

            IVector result = new SimpleVector(Size);
            if (UseDiagonal)
            {

                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        if (i <= j)
                            result[i] += d_matrix[i, j] * x[j];
                    }
                }
            }
            else
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        if (i < j)
                            result[i] += d_matrix[i, j] * x[j];
                    }
                }
            }
            return result;
        }
        protected IVector MultT(IVector x, bool UseDiagonal)
        {
            if (this.Size != x.Size)
            {
                throw new DifferentSizeException("Не удалось выполнить LU-разложение");
            }

            IVector result = new SimpleVector(Size);
            if (UseDiagonal)
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        result[i] += d_matrix[j, i] * x[j];
                    }
                }
            }
            else
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        if (i != j)
                            result[i] += d_matrix[j, i] * x[j];
                    }
                }
            }
            return result;
        }

        protected IVector SolveLT(IVector x, bool UseDiagonal = true)
        {
            IVector result = new SimpleVector(Size);
            for (int i = 0; i < Size; i++)
                result[i] = x[i];

            if (!UseDiagonal)
            {
                if (Math.Abs(x[Size - 1]) < EQU_TO_ZERO)
                {
                    result[Size - 1] = 0;
                }
                else
                    throw new SlaeNotCompatipableException("Система неразрешима.");
                return null;
            }
            for (int i = Size - 1; i >= 0; i--)
            {
                int line_length = i;
                try
                {
                    if (extraDiagVal == 0)
                        result[i] /= this[i, i];
                    else
                        result[i] /= extraDiagVal;
                }
                catch (DivideByZeroException)
                {
                    throw new SlaeNotCompatipableException("Произошло деление на ноль.");
                }
                for (int j = 0; j < line_length; j++)
                {
                    result[j] -= result[i] * this[i,j];
                }

            }
            return result;
        }

        protected IVector SolveUT(IVector x, bool UseDiagonal = true)
        {
            IVector result = new SimpleVector(Size);
            if (!UseDiagonal)
            {
                if (Math.Abs(x[0]) < EQU_TO_ZERO)
                {
                    result[0] = 0;
                }
                else
                    throw new SlaeNotCompatipableException("Система неразрешима.");
            }
            for (int i = 0; i < Size; i++)
            {
                result[i] = x[i];
                for (int j = 0; j < i; j++)
                    result[i] -= result[j] * this[j, i];
                try
                {
                    result[i] /= this[i, i];
                }
                catch (DivideByZeroException)
                {
                    throw new SlaeNotCompatipableException("Произошло деление на ноль.");
                }
            }
            return result;
        }

        protected IVector MultLT(IVector x, bool UseDiagonal = true)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");

            IVector result = new SimpleVector(Size);
            if (UseDiagonal)
            {
                if (extraDiagVal == 0)
                {
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                        {
                            if (i >= j)
                            {
                                result[j] += d_matrix[i, j] * x[i];
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Size; i++)
                    {
                        for (int j = 0; j < Size; j++)
                        {
                            if (i > j)
                            {
                                result[j] += d_matrix[i, j] * x[i];
                                continue;
                            }
                            if (i == j)
                            {
                                result[j] += extraDiagVal * x[i];
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        if (i > j)
                            result[j] += d_matrix[i, j] * x[i];
                    }
                }
            }
            return result;
        }

        protected IVector MultUT(IVector x, bool UseDiagonal = true)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");

            IVector result = new SimpleVector(Size);
            if (UseDiagonal)
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        if (i <= j)
                            result[j] += d_matrix[i, j] * x[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        if (i < j)
                            result[j] += d_matrix[i, j] * x[i];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Усножение вектора на диагональ матрицы (диагональную матрицу)
        /// </summary>
        /// <param name="a">Умножаемый вектор</param>
        public IVector SolveD(IVector a)
        {
            if (this.Size != a.Size)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");
            IVector result = new SimpleVector(Size);
            for (int i = 0; i < Size; i++)
                result[i] = a[i] / this[i, i];

            return result;
        }

        public object Clone()
        {
            return new DenseMatrix(d_matrix, isSymmetric);
        }

        public static void localtest()
        {
            double[,] val = new double[4, 4] { {1, 4, 4, 4 }, { 1, 1, 3, 3 }, { 1, 1, 1, 2 }, { 1, 1, 1, 1 } };

            IMatrix mar = new DenseMatrix(val);
            IPreconditioner pre = new LUPreconditioner(mar);

            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector y = mar.Mult(x, true);
            //should be { 37, 24, 14, 10 }

            y = pre.MultL(x);
            //shold be { 1, 3, 6, 10 }

            y = pre.MultU(x);
            //shold be { 37, -13, -10, -4 }

            IVector z = (IVector)y.Clone();
            //should do not crash

            z = pre.SolveL(x);
            //should be { 1, 1, 1, 1 }

            z = pre.SolveU(x);
            //should be { 13, 0.5, 0.5, -4 }

            IVector ut = mar.T.MultU(x);
            //should be { 1, 6, 13, 20 }

            ut = mar.T.MultU(x, false);
            //should be { 0, 4, 10, 16 }

            IVector lt = mar.T.MultL(x);
            //should be { 10, 9, 7, 4 }
        }

        public IVector MultD(IVector a)
        {
            IVector result = new SimpleVector(Size);
            for (int i = 0; i < this.Size; i++)
                result[i] = a[i] * this[i, i];
            return result;

        }

        public DenseMatrix(Dictionary<string, string> paths, bool isSymmetric = false)
        {
            this.isSymmetric = isSymmetric;
            StreamReader reader;
            try
            {
                reader = new StreamReader(paths["dense"]);
            }
            catch (System.Collections.Generic.KeyNotFoundException e)
            {
                throw new CannotFillMatrixException(string.Format("Отсутствует информация о расположении файла 'dense'."));
            }

            string line;
            string[] subline;
            line = reader.ReadLine();
            subline = line.Split(' ', '\t', ',');
            int n;
            try
            {
                n = Convert.ToInt32(subline[0]);
            }
            catch
            {
                throw new CannotFillMatrixException(string.Format("Файл 'dense' не соответствует требуемому формату. Первая строка не содержит размер матрицы."));
            }
            this.Size = n;
            d_matrix = new double[Size, Size];
            double val;
            int i = 0;
            try
            {
                for (i = 0; i < n; i++)
                {
                    line = reader.ReadLine();
                    subline = line.Split(' ', '\t', ',');

                    if (isSymmetric)
                        for (int j = 0; j <= i; j++)
                        {
                            val = Convert.ToDouble(subline[j]);
                            this[i, j] = val;
                            this[j, i] = val;
                        }
                    else
                        for (int j = 0; j < n; j++)
                        {
                            val = Convert.ToDouble(subline[j]);
                            this[i, j] = val;
                        }

                }
            }
            catch
            {
                throw new CannotFillMatrixException(string.Format("Количество элементов в строке меньше чем размер матрицы.", i));
            }
        }
        public bool CheckCompatibility(IVector x)
        {
            int j;
            int firstNotZero;
            double coef;
            // Сверхнеоптимальный код
            // Желательно читать с закрытыми глазами
            for (int line = 0; line < Size; line++)
            {
                for (int line2 = line + 1; line2 < Size; line2++)
                {
                    for (firstNotZero = 0; firstNotZero < Size; firstNotZero++)
                        if (this[line, firstNotZero] != 0 || this[line2, firstNotZero] != 0)
                            break;

                    if (firstNotZero != Size)
                    {
                        // Если ненулевой элемент во второй строчке
                        if (this[line, firstNotZero] == 0)
                        {
                            coef = this[line, firstNotZero] / this[line2, firstNotZero];
                            // Проверка линейно зависимости строк матрицы
                            for (j = firstNotZero + 1; j < Size; j++)
                            {
                                if (this[line, j] - coef * this[line2, j] != 0)
                                    break;
                            }
                            // Если строки линейно зависимы, то проверка соответствующих элементов вектора
                            if (j == Size)
                            {
                                if (x[line] - x[line2] * coef == 0)
                                    return false;
                            }
                        }
                        // Если ненулевой элемент в первой строчке
                        else
                        {
                            coef = this[line2, firstNotZero] / this[line, firstNotZero];
                            // Проверка линейно зависимости строк матрицы
                            for (j = firstNotZero + 1; j < Size; j++)
                            {
                                if (this[line2, j] - coef * this[line, j] != 0)
                                    break;
                            }
                            // Если строки линейно зависимы, то проверка соответствующих элементов вектора
                            if (j == Size)
                            {
                                if (x[line2] - x[line] * coef == 0)
                                    return false;
                            }
                        }
                    }
                    else
                    {
                        // Если нулевой строке матрицы соответствует ненулевой элемент вектора
                        if (x[line2] != 0 || x[line] != 0)
                            return false;
                    }
                }
            }
            return true;
        }
    }
}