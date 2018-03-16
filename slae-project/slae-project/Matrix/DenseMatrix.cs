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
namespace slae_project.Matrix
{
    public class DenseMatrix : IMatrix
    {
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
            public IVector SolveD(IVector x) => Matrix.SolveLT(x);
            public void MakeLU() => throw new NotImplementedException();
            public object Clone() => Matrix.Clone();
        }
        // Матрица
        private double[,] d_matrix;
        double extraDiagVal = 0;

        bool LU_was_made = false;
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
                    LU_was_made = false;
                }
            }
        }
        public int Size { get; }
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
        public DenseMatrix(double[,] val)
        {
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

        public DenseMatrix(int Size)
        {
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

        public IVector Mult(IVector x, bool UseDiagonal = true)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");

            IVector result = new SimpleVector(Size);
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    result[i] += d_matrix[i, j] * x[j];
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
                    throw new CannotSolveSLAEExcpetion("Система неразрешима.");
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
                    throw new CannotSolveSLAEExcpetion("Произошло деление на ноль.");
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
                    throw new CannotSolveSLAEExcpetion("Система неразрешима.");
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
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    result[i] += d_matrix[j, i] * x[j];
                }
            }
            return result;
        }

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
                    if (Math.Abs(x[Size - 1]) < EQU_TO_ZERO)
                    {
                        result[Size - 1] = 0;
                    }
                    else
                        throw new CannotSolveSLAEExcpetion("Система неразрешима.");
                    return null;
                }
                for (int i = Size - 1; i >= d; i--)
                {
                    var line = L[i];
                    var offset = Size - line.Length;
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
                        throw new CannotSolveSLAEExcpetion("Произошло деление на ноль.");
                    }
                    for (int j = 0; j < line.Length - d - 1; j++)
                    {
                        result[j] -= result[i] * L[i][j];
                    }

                }
                return result;
            }
            throw new LUFailException();
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
                    if (Math.Abs(x[0]) < EQU_TO_ZERO)
                    {
                        result[0] = 0;
                    }
                    else
                        throw new CannotSolveSLAEExcpetion("Система неразрешима.");
                }
                for (int i = 0; i < Size - d; i++)
                {
                    var line = U[i];
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
                        throw new CannotSolveSLAEExcpetion("Произошло деление на ноль.");
                    }

                    for (int j = 1 + d; j < line.Length; j++)
                    {
                        result[j + i] -= result[i] * U[i][j];
                    }

                }
                return result;
            }
            throw new LUFailException();
        }

        protected IVector MultLT(IVector x, bool UseDiagonal = true)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
                return CommoLUMult(x, L, UseDiagonal, true);
            throw new LUFailException();
        }

        protected IVector MultUT(IVector x, bool UseDiagonal = true)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
                return CommoLUMult(x, U, UseDiagonal, true);
            throw new LUFailException();
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
            DenseMatrix copy = new DenseMatrix(d_matrix);
            return copy;
        }

        public static void localtest()
        {
            double[,] val = new double[4, 4] { {1, 4, 4, 4 }, { 1, 1, 3, 3 }, { 1, 1, 1, 2 }, { 1, 1, 1, 1 } };

            IMatrix mar = new DenseMatrix(val);
            IPreconditioner pre = new LUPreconditioner(mar);

            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector y = mar.Mult(x, true);
            //should be { 37 24 14 10 }

            y = pre.MultL(x);
            //shold be { 1 3 6 10}

            y = pre.MultU(x);
            //shold be { 37 -13 -10 -4 }

            IVector z = (IVector)y.Clone();
            z = mar.SolveL(x);
            z = mar.SolveU(z);
            //should be {5 0 0 -1}

            y = mar.T.SolveU(x);
            y = mar.T.SolveL(y);
            //should be {1/3 1/2 1 -5/6}
        }
    }
}