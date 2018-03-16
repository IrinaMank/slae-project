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
            public IVector Mult(IVector x, bool UseDiagonal) => Matrix.MultT(x, UseDiagonal);
            public IVector MultU(IVector x, bool UseDiagonal) => Matrix.MultUT(x, UseDiagonal);
            public IVector SolveU(IVector x, bool UseDiagonal) => Matrix.SolveUT(x, UseDiagonal);
            public IVector SolveD(IVector x) => Matrix.SolveD(x);
            public object Clone() => Matrix.Clone();
            public void MakeLU()
            {
                throw new NotImplementedException();
            }

            public void MakeLUSeidel()
            {
                throw new NotImplementedException();
            }
        }
        // Элементы матрицы
        Dictionary<(int i, int j), double> elements = new Dictionary<(int i, int j), double>();
        //Идентефикатор выполненности LU - разложения
        // = false после любого изменения матрицы
        // = true после выполнения LU - разложения
        bool LU_was_made = false;
        // Значение, начиная с которого любое число считается равным нулю
        private double EQU_TO_ZERO { get; } = 1e-10;
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
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");

            int maxij = 0;
            for (int i = 0; i < val.Length; i++)
            {
                this.elements.Add((coord[i][0], coord[i][1]), val[i]);
                if (coord[i][0] > maxij)
                    maxij = coord[i][0];
                if (coord[i][1] > maxij)
                    maxij = coord[i][1];
            }
            this.Size = maxij + 1;
        }
        /// <summary>
        /// Инициализация матрицы массивом координат и массивом значений
        /// </summary>
        /// <param name="coord">Массив координат размерности (N,2)</param>
        /// <param name="val">Массив значений</param>
        public CoordinateMatrix((int x, int y)[] coord, double[] val)
        {
            if (coord.Length != val.Length)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");
            int maxij = 0;
            for (int i = 0; i < val.Length; i++)
            {
                this.elements.Add(coord[i], val[i]);
                if (coord[i].x > maxij)
                    maxij = coord[i].x;
                if (coord[i].x > maxij)
                    maxij = coord[i].y;
            }
            this.Size = maxij + 1;
        }
        public CoordinateMatrix(int size)
        {
            this.Size = size;
        }
        public CoordinateMatrix(Dictionary<(int i, int j), double> elemets, int size)
        {
            this.elements = new Dictionary<(int i, int j), double>(elements);
            this.Size = size;
        }
        public IVector Mult(IVector x, bool UseDiagonal = true)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");

            IVector result = new SimpleVector(Size);
            if (UseDiagonal)
            {
                foreach (var el in elements)
                {
                    result[el.Key.i] += el.Value * x[el.Key.j];
                }
            }
            else
            {
                foreach (var el in elements)
                {
                    if (el.Key.i != el.Key.j)
                        result[el.Key.i] += el.Value * x[el.Key.j];
                }
            }
            return result;
        }
        //TODO: Написать эффективный алгоритм
        //С учетом того, что портрет не сохраняется
        public void MakeLU()
        {
            try
            {
                double el;
                for (int i = 0; i < Size; i++)
                {
                    el = this[0, i];
                    if (el != 0)
                        this[0, i] = el / this[0, 0];
                }

                double sum;
                for (int i = 1; i < Size; i++)
                {
                    for (int j = i; j < Size; j++)
                    {
                        sum = 0;
                        for (int k = 0; k < i; k++)
                            sum += this[i, k] * this[k, j];

                        el = this[i, j] - sum;
                        if (el != 0)
                            this[i, j] = el;

                        sum = 0;
                        for (int k = 0; k < i; k++)
                            sum += this[j, k] * this[k, i];

                        el = this[j, i] - sum;
                        if (el != 0)
                            this[j, i] = (el) / this[i, 0];
                    }

                }
                LU_was_made = true;
            }
            catch (DivideByZeroException)
            {
                LU_was_made = false;
                throw new LUFailException();
            }
        }
        //public void MakeLUold()
        //{ //Выделение памяти
        //    L = new List<double[]> { };
        //    U = new List<double[]> { };
        //    for (int i = 1; i <= Size; i++)
        //    {
        //        L.Add(new double[i]);
        //        U.Add(new double[Size - i + 1]);
        //    }
        //    // Разложение
        //    try
        //    {

        //        for (int i = 0; i < Size; i++)
        //        {
        //            L[i][0] = this[i, 0];
        //            U[0][i] = this[0, i] / L[0][0];
        //        }

        //        double sum;
        //        for (int i = 1; i < Size; i++)
        //        {
        //            for (int j = i; j < Size; j++)
        //            {
        //                sum = 0;
        //                for (int k = 0; k < i; k++)
        //                    sum += L[i][k] * U[k][j - k];

        //                U[i][j - i] = this[i, j] - sum;
        //                sum = 0;
        //                for (int k = 0; k < i; k++)
        //                    sum += L[j][k] * U[k][i - k];

        //                L[j][i] = (this[j, i] - sum) / U[i][0];
        //            }

        //        }
        //        LU_was_made = true;
        //    }
        //    catch (DivideByZeroException)
        //    {
        //        LU_was_made = false;
        //        throw new LUFailException();
        //    }
        //}
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
                    throw new CannotSolveSLAEExcpetion("Система неразрешима.");
            }
            for (int i = 0; i < Size; i++)
            {
                result[i] = x[i];
                for (int j = 0; j < i; j++)
                    result[i] -= result[j] * this[i, j];
                try
                {
                    result[i] /= this[i, i];
                }
                catch (DivideByZeroException)
                {
                    throw new CannotSolveSLAEExcpetion("Произошло деление на ноль.");
                }
            }
            return result;

        }
        //Метод еще не готов
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
                    throw new CannotSolveSLAEExcpetion("Система неразрешима.");
            }
            for (int i = Size - 1; i >= 0; i--)
            {
                int line_length = Size - i;
                result[i] = x[i];
                for (int j = i + 1; j < line_length; j++)
                    result[i] -= result[j] * this[i, j];
                try
                {
                    result[i] /= this[i, i];
                }
                catch (DivideByZeroException)
                {
                    throw new CannotSolveSLAEExcpetion("Произошло деление на ноль.");
                }
            }
            return result;
        }
        public IVector MultL(IVector x, bool UseDiagonal = true)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");

            IVector result = new SimpleVector(Size);
            foreach (var el in elements)
                if (el.Key.i <= el.Key.j)
                    result[el.Key.i] += el.Value * x[el.Key.j];

            return result;

        }
        public IVector MultU(IVector x, bool UseDiagonal = true)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");

            IVector result = new SimpleVector(Size);
            foreach (var el in elements)
                if (el.Key.i > el.Key.j)
                    result[el.Key.i] += el.Value * x[el.Key.j];

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
                foreach (var el in elements)
                {
                    result[el.Key.j] += el.Value * x[el.Key.i];
                }
            }
            else
            {
                foreach (var el in elements)
                {
                    if (el.Key.i != el.Key.j)
                        result[el.Key.j] += el.Value * x[el.Key.i];
                }
            }
            return result;
        }
        //Уверен все можно свести к одному методу
        //Но зачем, когда и так неплохо работает?
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
                    throw new CannotSolveSLAEExcpetion("Система неразрешима.");
                return null;
            }
            for (int i = Size - 1; i >= 0; i--)
            {
                int line_length = i;
                try
                {
                    result[i] /= this[i, line_length - 1];
                }
                catch (DivideByZeroException)
                {
                    throw new CannotSolveSLAEExcpetion("Произошло деление на ноль.");
                }
                for (int j = 0; j < line_length - 1; j++)
                {
                    result[j] -= result[i] * this[i,j];
                }

            }
            return result;
        }
        protected IVector SolveUT(IVector x, bool UseDiagonal = true)
        {
            
                IVector result = new SimpleVector(Size);
                for (int i = 0; i < Size; i++)
                    result[i] = x[i];

                if (!UseDiagonal)
                {
                    if (Math.Abs(x[0]) < EQU_TO_ZERO)
                    {
                        result[0] = 0;
                    }
                    else
                        throw new CannotSolveSLAEExcpetion("Система неразрешима.");
                }
                for (int i = 0; i < Size; i++)
                {
                    int line_length = Size - i;
                    try
                    {
                        result[i] /= this[i,i];
                    }
                    catch (DivideByZeroException)
                    {
                        throw new CannotSolveSLAEExcpetion("Произошло деление на ноль.");
                    }

                    for (int j = i + 1; j < line_length; j++)
                    {
                        result[j] -= result[i] * this[i, j];
                    }

                }
                return result;
        }
        protected IVector MultLT(IVector x, bool UseDiagonal = true)
        {
            if (this.Size != x.Size)
            {
                throw new DifferentSizeException("Не удалось выполнить LU-разложение");
            }
            IVector result = new SimpleVector(Size);
                foreach (var el in elements)
                {
                    if(el.Key.i<=el.Key.j)
                        result[el.Key.j] += el.Value * x[el.Key.i];
                }
            return result;
        }
        protected IVector MultUT(IVector x, bool UseDiagonal = true)
        {
            if (this.Size != x.Size)
            {
                throw new DifferentSizeException("Не удалось выполнить LU-разложение");
            }
            IVector result = new SimpleVector(Size);
            foreach (var el in elements)
            {
                if (el.Key.i > el.Key.j)
                    result[el.Key.j] += el.Value * x[el.Key.i];
            }
            return result;
        }

        public static void localtest()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IPreconditioner pre = new LUPreconditioner(mar);


            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector y = mar.Mult(x, true);
            IVector z = (IVector)y.Clone();

            z = mar.SolveL(x);
            z = mar.SolveU(z);
            //should be { 5 0 0 - 1}

            y = mar.T.SolveU(x);
            y = mar.T.SolveL(y);
            //should be {1/3 1/2 1 -5/6}

            IVector ut = mar.T.MultU(x);
            //should be {1 -2 -4 -5}

            IVector lt = mar.T.MultL(x);
            //should be {10 9 7 4}
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
            {
                result[i] = a[i] / this[i, i];
            }
            return result;
        }

        public object Clone()
        {
            return new CoordinateMatrix(this.elements, Size);
        }
    }
}