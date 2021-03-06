﻿using System;
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
    public class CoordinateMatrix : IMatrix
    {
        public static Dictionary<string, string> requiredFileNames => new Dictionary<string, string>
        {
            {
                "elements",
                "Файл должен содержать в первой строке количество оставшихся строк в файле." +
                " В каждой последующей строке содержится запись формата <i j value> " +
                "где i - значение типа integer - номер строки элемента матрицы, "+
                "j - значение типа integer - номер столбца элемента матрицы, "+
                "value - значение типа double - значение (i,j)-го элемента матрицы."
            },
            {
                "size",
                "Файл должен содержать одно единственное значение типа integer - размерность матрицы."
            },
        };



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
            public void MakeLU() => Matrix.MakeLU();

            public IVector MultD(IVector a)
            {
                throw new NotImplementedException();
            }
        }
        // Элементы матрицы
        protected Dictionary<(int i, int j), double> elements = new Dictionary<(int i, int j), double>();

        //Переменная, необходимая для реализации возможности наличия в матрицы двух диагоналей ( например в случае LU - разложенной матрицы)
        // Если extraDiagVal = 0, то считается, что в матрице одна диагональ
        // Если extraDiagVal != 0, то считается, что нижний треугольник матрицы содержит диагональ, заполненную значениями extraDiagVal
        double extraDiagVal = 0;
        bool isSymmetric = false;
        // Значение, начиная с которого любое число считается равным нулю
        protected double EQU_TO_ZERO { get; } = 1e-10;
        public double this[int i, int j]
        {

            get
            {
                if (i < this.Size && j < this.Size && i >= 0 && j >= 0)
                {
                    (int, int) ij;
                    if (i < j && isSymmetric)
                        ij = (j, i);
                    else
                        ij = (i, j);

                    if (elements.ContainsKey(ij))
                        return elements[ij];
                    else
                        return 0;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (i < this.Size && j < this.Size && i >= 0 && j >= 0)
                {
                    if (value != 0)
                    {
                        (int, int) ij;

                        if (i < j && isSymmetric)
                            ij = (j, i);
                        else
                            ij = (i, j);

                        elements[ij] = value;
                    }
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        // Предполагаются только квадратные матрицы
        public int Size { get; private set; }
        public ILinearOperator Transpose => new TransposeIllusion { Matrix = this };
        public ILinearOperator T => new TransposeIllusion { Matrix = this };

        //TODO: Метод и правда должен что-то возвращать
        public IVector Diagonal
        {
            get
            {
                IVector diag = new SimpleVector(Size);
                foreach (var el in this)
                {
                    if (el.col == el.row)
                        diag[el.row] = el.value;
                }
                return diag;
            }
        }

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
        public CoordinateMatrix(int[][] coord, double[] val, bool isSymmetric = false)
        {
            if (coord.Length != val.Length)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");
            this.isSymmetric = isSymmetric;
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
        public CoordinateMatrix((int x, int y)[] coord, double[] val, bool isSymmetric = false)
        {
            if (coord.Length != val.Length)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");
            this.isSymmetric = isSymmetric;
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
        public CoordinateMatrix(Dictionary<(int i, int j), double> elemets, int size, bool isSymmetric = false)
        {
            this.isSymmetric = isSymmetric;
            this.elements = elemets.ToDictionary(entry => entry.Key, entry => entry.Value);
            this.Size = size;
        }

        public CoordinateMatrix(bool isSymmetric = false)
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
                if (isSymmetric)
                {
                    foreach (var el in elements)
                    {
                        result[el.Key.i] += el.Value * x[el.Key.j];
                        result[el.Key.j] += el.Value * x[el.Key.i];
                    }
                    for (int i = 0; i < result.Size; i++)
                        result[i] -= x[i];
                }
                else
                    foreach (var el in elements)
                    {
                        result[el.Key.i] += el.Value * x[el.Key.j];
                    }
            }
            else
            {
                if (isSymmetric)
                    foreach (var el in elements)
                    {
                        if (el.Key.i != el.Key.j)
                        {
                            result[el.Key.i] += el.Value * x[el.Key.j];
                            result[el.Key.j] += el.Value * x[el.Key.i];
                        }
                    }
                else
                    foreach (var el in elements)
                    {
                        if (el.Key.i != el.Key.j)
                            result[el.Key.i] += el.Value * x[el.Key.j];
                    }

            }
            return result;
        }
        private void CastToNotSymm()
        {
            if (isSymmetric)
            {
                isSymmetric = false;
                for (int i = 0; i < Size; i++)
                    for (int j = 0; j < i; j++)
                        this[j, i] = this[i, j];
            }


        }
        //TODO: Написать эффективный алгоритм
        //С учетом того, что портрет не сохраняется
        public void MakeLU()
        {
            try
            {
                CastToNotSymm();
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
                        if (Math.Abs(this[k, k]) < 1e-10)
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
                    foreach (var el in elements)
                    {
                        if (el.Key.i >= el.Key.j)
                        {
                            result[el.Key.i] += el.Value * x[el.Key.j];
                        }
                    }
                }
                else
                {
                    foreach (var el in elements)
                    {
                        if (el.Key.i > el.Key.j)
                        {
                            result[el.Key.i] += el.Value * x[el.Key.j];
                            continue;
                        }
                        if (el.Key.i == el.Key.j)
                        {
                            result[el.Key.i] += extraDiagVal * x[el.Key.j];
                        }
                    }
                }
            }
            else
            {
                foreach (var el in elements)
                    if (el.Key.i > el.Key.j)
                        result[el.Key.i] += el.Value * x[el.Key.j];
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
                if (isSymmetric)
                {
                    foreach (var el in elements)
                        result[el.Key.j] += el.Value * x[el.Key.i];
                }
                else
                {
                    foreach (var el in elements)
                        if (el.Key.i <= el.Key.j)
                            result[el.Key.i] += el.Value * x[el.Key.j];
                }
            }
            else
            {
                foreach (var el in elements)
                    if (el.Key.i < el.Key.j)
                        result[el.Key.i] += el.Value * x[el.Key.j];
            }
            return result;
        }
        protected IVector MultT(IVector x, bool UseDiagonal)
        {
            if (this.Size != x.Size)
            {
                throw new DifferentSizeException("Не удалось выполнить LU-разложение");
            }
            if (isSymmetric)
                return this.Mult(x, UseDiagonal);

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
                    result[j] -= result[i] * this[i, j];
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

            if (isSymmetric)
                return this.MultU(x, UseDiagonal);

            IVector result = new SimpleVector(Size);
            if (UseDiagonal)
            {
                if (extraDiagVal == 0)
                {
                    foreach (var el in elements)
                    {
                        if (el.Key.i >= el.Key.j)
                        {
                            result[el.Key.j] += el.Value * x[el.Key.i];
                        }
                    }
                }
                else
                {
                    foreach (var el in elements)
                    {
                        if (el.Key.i > el.Key.j)
                        {
                            result[el.Key.j] += el.Value * x[el.Key.i];
                            continue;
                        }
                        if (el.Key.i == el.Key.j)
                        {
                            result[el.Key.j] += extraDiagVal * x[el.Key.i];
                        }
                    }
                }
            }
            else
            {
                foreach (var el in elements)
                    if (el.Key.i > el.Key.j)
                        result[el.Key.j] += el.Value * x[el.Key.i];
            }
            return result;
        }
        protected IVector MultUT(IVector x, bool UseDiagonal = true)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");

            if (isSymmetric)
                return this.MultL(x, UseDiagonal);
            IVector result = new SimpleVector(Size);
            if (UseDiagonal)
            {
                foreach (var el in elements)
                    if (el.Key.i <= el.Key.j)
                        result[el.Key.j] += el.Value * x[el.Key.i];
            }
            else
            {
                foreach (var el in elements)
                    if (el.Key.i < el.Key.j)
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

            IMatrix mar = new CoordinateMatrix(new Dictionary<string, string> { { "size", "size.txt" }, { "elements", "elements.txt" } });

            IPreconditioner pre = new LUPreconditioner(mar);

            IVector x = new SimpleVector(new double[5] { 1, 2, 3, 4, 5 });

            IVector y = mar.Mult(x, true);
            //should be { 37 24 14 10 }

            y = pre.MultL(x);
            //shold be { 1 3 6 10}

            y = pre.MultU(x);
            //shold be { 37 -13 -10 -4 }


            IVector z = (IVector)y.Clone();
            //should do not crash


            z = pre.SolveL(x);
            //should be { 1 1 1 1 }

            y = pre.SolveU(z);
            //should be { 13 0.5 0.5 -4}

            IVector ut = mar.T.MultU(x);
            //should be {1 6 13 20}

            ut = mar.T.MultU(x, false);
            //should be {0 4 10 16}

            IVector lt = mar.T.MultL(x);
            //should be {10 9 7 4}
        }
        public static void localtestsymm()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(new Dictionary<string, string> { { "size", "size.txt" }, { "elements", "elements.txt" } }, true);

            IPreconditioner pre = new LUPreconditioner(mar);

            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector y = mar.Mult(x, true);
            //should be { 10 10 10 10 }

            y = mar.MultL(x, true);
            //should be { 1 12 39 85 }


            IVector z = (IVector)y.Clone();
            //should do not crash


            z = pre.SolveL(x);
            y = pre.SolveU(z);
            //should be { 5 0 0 -1}

            IVector ut = mar.T.MultU(x);
            //should be {1 6 13 20}

            ut = mar.T.MultU(x, false);
            //should be {0 4 10 16}

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
            return new CoordinateMatrix(this.elements, Size, isSymmetric);
        }

        public IVector MultD(IVector a)
        {
            IVector result = new SimpleVector(this.Size);
            foreach (var el in this)
            {
                if (el.col == el.row)
                    result[el.col] = a[el.col] * el.value;
            }
            return result;
        }


        public int CheckCompatibility(IVector x)
        {
            int j;
            int firstNotZero;
            double coef;
            int toReturn = MatrixConstants.SLAE_OK;
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
                        // Если первый ненулевой элемент во второй строчке
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
                                if (x[line] - x[line2] * coef != 0)
                                    return MatrixConstants.SLAE_INCOMPATIBLE;
                                else
                                    toReturn = MatrixConstants.SLAE_MORE_ONE_SOLUTION;
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
                                if (x[line2] - x[line] * coef != 0)
                                    return MatrixConstants.SLAE_INCOMPATIBLE;
                                else
                                    toReturn = MatrixConstants.SLAE_MORE_ONE_SOLUTION;
                            }
                        }
                    }
                    else
                    {
                        // Если нулевой строке матрицы соответствует ненулевой элемент вектора
                        if (x[line2] != 0 || x[line] != 0)
                            return MatrixConstants.SLAE_INCOMPATIBLE;
                        else
                            toReturn = MatrixConstants.SLAE_MORE_ONE_SOLUTION;
                    }
                }
            }
            return toReturn;
        }

        public CoordinateMatrix(Dictionary<string, string> paths, bool isSymmetric = false)
        {
            this.isSymmetric = isSymmetric;
            //Считывание размера матрицы
            StreamReader reader;
            try
            {
                reader = new StreamReader(paths["size"]);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                throw new CannotFillMatrixException(string.Format("Отсутствует информация о расположении файла 'size'."));
            }
            catch
            {
                throw new CannotFillMatrixException(string.Format("Отсутствует файл 'size' по указанному пути '{0}'", paths["size"]));
            }

            string line;
            string[] subline;
            line = reader.ReadLine();
            subline = line.Split(' ', '\t', ',');
            try
            {
                this.Size = Convert.ToInt32(subline[0]);
            }
            catch
            {
                throw new CannotFillMatrixException(string.Format("Файл 'size' содержит не целочисленное значение."));
            }

            //Считывание элементов массива
            try
            {
                reader = new StreamReader(paths["elements"]);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                throw new CannotFillMatrixException(string.Format("Отсутствует информация о расположении файла 'elements'."));
            }
            catch
            {
                throw new CannotFillMatrixException(string.Format("Отсутствует файл 'elements' по указанному пути '{0}'", paths["size"]));
            }

            line = reader.ReadLine();
            subline = line.Split(' ', '\t', ',');
            int n;
            try
            {
                n = Convert.ToInt32(subline[0]);
            }
            catch
            {
                throw new CannotFillMatrixException(string.Format("Файл 'elements' не соответствует требуемому формату. Первая строка не содержит количество строк в файле."));
            }

            int i, j;
            double val;
            int k = 0;
            try
            {
                for (k = 0; k < n; k++)
                {
                    line = reader.ReadLine();
                    subline = line.Split(' ', '\t', ',');
                    i = Convert.ToInt32(subline[0]);
                    j = Convert.ToInt32(subline[1]);
                    val = Convert.ToDouble(subline[2]);
                    this[i, j] = val;
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new CannotFillMatrixException(string.Format("Индекс, указанный в строке {0} не соответствует указанному размеру матрицы", k + 2));
            }
            catch
            {
                throw new CannotFillMatrixException(string.Format("Строка #{0} в файле 'elements' не соответствует формату", k + 2));
            }
            reader.Close();
        }
    }
}