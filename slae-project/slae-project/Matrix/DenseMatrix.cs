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
        }
        // Матрица
        private double[,] d_matrix;

        bool LU_was_made = false;
        private List<double[]> L;
        private List<double[]> U;
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
                                sum += L[i][k] * U[k][j - k];

                            L[i][j] = this[i, j] - sum;
                        }

                        if (j >= i)
                        {
                            sum = 0;
                            for (int k = 0; k < i; k++)
                                sum += L[i][k] * U[k][j - k];

                            U[i][j - i] = (this[i, j] - sum) / L[i][i];
                        }
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
                    if (Math.Abs(x[0]) < EQU_TO_ZERO)
                    {
                        result[0] = 0;
                    }
                    else
                        throw new CannotSolveSLAEExcpetion("Система неразрешима.");
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
                        throw new CannotSolveSLAEExcpetion("Произошло деление на ноль.");
                    }
                }
                return result;
            }
            throw new LUFailException();
        }

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
                    if (Math.Abs(x[Size - 1]) < EQU_TO_ZERO)
                    {
                        result[Size - 1] = 0;
                    }
                    else
                        throw new CannotSolveSLAEExcpetion("Система неразрешима.");
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
                        throw new CannotSolveSLAEExcpetion("Произошло деление на ноль.");
                    }
                }
                return result;
            }
            throw new LUFailException();
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
            throw new LUFailException();
        }

        public IVector MultU(IVector x, bool UseDiagonal = true)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
                return CommoLUMult(x, U, UseDiagonal, false);
            throw new LUFailException();
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

        //решение СЛАУ диагональ
        public IVector SolveD(IVector x)
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            DenseMatrix copy = new DenseMatrix(d_matrix);
            return copy;
        }
    }
}