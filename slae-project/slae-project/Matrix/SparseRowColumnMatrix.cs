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
    class SparseRowColumnMatrix : IMatrix
    {
        //что делать с транспонированием?
        class TransposeIllusion : ILinearOperator
        {
            public SparseRowColumnMatrix Matrix { get; set; }
            public ILinearOperator Transpose => Matrix;
            public ILinearOperator T => Matrix;
            public IVector Diagonal { get; }
            public int Size => Matrix.Size;
            public IVector MultL(IVector x, bool UseDiagonal) => Matrix.MultLT(x, UseDiagonal);
            public IVector SolveL(IVector x, bool UseDiagonal) => Matrix.SolveLT(x, UseDiagonal);
            public IVector Mult(IVector x, bool UseDiagonal) => Matrix.MultT(x,UseDiagonal);
            public IVector MultU(IVector x, bool UseDiagonal) => Matrix.MultUT(x, UseDiagonal);
            public IVector SolveU(IVector x, bool UseDiagonal) => Matrix.SolveUT(x, UseDiagonal);
            public IVector SolveD(IVector x) => Matrix.SolveD(x);
            public void MakeLU() => Matrix.MakeLU();
            public object Clone() => Matrix.Clone();
        }


        //необходимые вектора для реализации строчно-столбцового формата
        double[] di;
        int[] ig;
        int[] jg;
        double[] al;
        double[] au;

        bool LU_was_made = false;
        // портрет сохраняется
        private double[] L;
        private double[] U;
        private double[] DL;
        // диагональ для матрицы U
        private double[] DU;
        public double this[int i, int j]
        {
            get
            {
                bool down;
                if (i == j)
                    return di[i];
                int k;
                if (SearchPlaceInAlAu(i, j, out down, out k))
                    if (down) return al[k];
                    else return au[k];
                else return 0;
            }
            set
            {
                bool down;
                if (i == j)
                    di[i] = value;
                int k;
                if(SearchPlaceInAlAu(i, j, out down, out k))
                    if (down) al[k] = value;
                    else au[k] = value;
            }
        }

        private bool SearchPlaceInAlAu(int i, int j, out bool down, out int last)
        {
            down = true;
            if (i < j)
            {
                // свап переменных
                int k = i; i = j; j = k;
                down = false;
            }
            // бинарный поиск
            int first = ig[i];
            last = ig[i + 1] - 1;
            int mid;
            if (first > last)
                return false;
            while (first < last)
            {
                mid = first + (last - first) / 2;
                if (j <= jg[mid])
                    last = mid;
                else
                    first = mid + 1;
            }
            if (jg[last] == j)
                return true;
            else return false;
        }
        
        public int Size { get; }

        public ILinearOperator Transpose => new TransposeIllusion { Matrix = this };
        public ILinearOperator T => new TransposeIllusion { Matrix = this };
        

         public IVector Diagonal
         {
            get {
                IVector diag = new SimpleVector(this.di);
                return diag;
            } 
             
         }
        
        public IEnumerator<(double value, int row, int col)> GetEnumerator()
        {
            IEnumerable<(double value, int row, int col)> F()
            {
                for (int i = 0; i < Size; i++)
                {
                    yield return (di[i], i, i);
                    for (int j = ig[i]; j < ig[i + 1]; j++)
                    {
                        yield return (al[j], i, jg[j]);
                        yield return (au[j], jg[j], i);
                    }
                }
            }
            return F().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Инициализация матрицы массивами ig,jg,di,al,au
        /// </summary>
        /// <param name="ig">Индексы начала строк</param>
        /// <param name="jg">Номера столбцов для элементов</param>
        /// <param name="di">Диагональные элементы</param>
        /// <param name="al">Элементы нижней диагонали</param>
        /// <param name="au">Элементы верхней диагонали</param>
        public SparseRowColumnMatrix(int[] ig, int[] jg, double[] di, double[] al, double[] au)
        {
            this.di = di;
            this.ig = ig;
            this.jg = jg;
            this.al = al;
            this.au = au;
            this.Size = di.Length;
        }

        public SparseRowColumnMatrix(int n)
        {
            di = new double[n];
            ig = new int[n + 1];

            for(int i=0; i<n; i++)
            {
                di[i] = 0;
                ig[i] = 0; //в строках нет элементов
            }
            //остальные массивы не создаются, т.к. они пустые
        }

        public SparseRowColumnMatrix(CoordinateMatrix c_matrix)
        {
            //
            //int i, j;
            //double value;
            //foreach (var val in c_matrix)
            //{
            //    i = val.col;
            //    j = val.row;
            //    value = val.value;
            //}
            // не дописан, в ближайшем будущем будет написан
            // текущая проблема в формировании портрета
            // есть предполагаемое решение в виде списком для каждой строки
        }

        //Какие еще могут быть виды инициализации, кроме заданных векторов?
        public IVector Mult(IVector x, bool UseDiagonal)
        {
            //проверка на корректное умножение (размеры вектора и матрицы)
            //допустим все корректно
            int j;
            IVector result = new SimpleVector(Size);
            if(UseDiagonal)
                for (int i = 0; i < this.Size; i++)
                    result[i] = di[i] * x[i];
            for (int i = 0; i < this.Size; i++)
            {
                {
                    for (int k = ig[i]; k < ig[i + 1]; k++)
                    {
                        j = jg[k];
                        result[i] += al[k] * x[j];
                        result[j] += au[k] * x[i];
                    }
                }
            }
            return result;
        }

        // нуждается в серьезном тестировании, очень серьезном
        /// <summary>
        /// Создание LU разложения
        /// </summary>
        public void MakeLU()
        {
            // пусть опять же все корректно
            // Версия с отдельным выделением диагонали di

            double sum_l, sum_u, sum_d;

            L = new double[ig[Size]];
            U = new double[ig[Size]];
            DL = new double[Size];
            DU = new double[Size];

            for (int i = 0; i < ig[Size]; i++)
            {
                L[i] = al[i];
                U[i] = au[i];
            }
            for (int i = 0; i < Size; i++)
            {
                DL[i] = di[i];
                DU[i] = 1;
            }

            for (int k = 1, k1 = 0; k <= Size; k++, k1++)
            {
                sum_d = 0;

                int i_s = ig[k1], i_e = ig[k];

                for (int m = i_s; m < i_e; m++)
                {

                    sum_l = 0; sum_u = 0;
                    int j_s = ig[jg[m]], j_e = ig[jg[m] + 1];
                    for (int i = i_s; i < m; i++)
                    {
                        for (int j = j_s; j < j_e; j++)
                        {
                            if (jg[i] == jg[j])
                            {
                                sum_l += L[i] * U[j];
                                sum_u += L[j] * U[i];
                                j_s++;
                            }
                        }
                    }
                    L[m] = L[m] - sum_l;
                    U[m] = (U[m] - sum_u) / DL[jg[m]];
                    if (double.IsInfinity(U[m]))
                        throw new LUFailException("Ошибка при LU предобуславливании!");
                    sum_d += L[m] * U[m];
                }
                DL[k1] = DL[k1] - sum_d;
            }
            LU_was_made = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="UseDiagonal">различие в делении на диагональ</param>
        /// <returns></returns>
        public IVector SolveL(IVector x, bool UseDiagonal)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                    //проверка корректности
                if (this.Size != x.Size)
                    throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции SolveL");
                else
                {
                    IVector result = new SimpleVector(this.Size);
                    double sum;

                    for (int i = 0; i < this.Size; i++)
                    {
                        sum = 0;
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            sum += L[j] * result[jg[j]];
                        result[i] = (x[i] - sum);
                        if (UseDiagonal == true)
                            result[i] /= DL[i];
                    }
                    return result;
                }
            }
            else
                throw new CannotMultException("Ошибка. Невозможно выполнить функцию SolveL, т.к. не удалось сделать разложение LU.");

            // можно было сделать главное условие if(UseDiagonal==true) и разбиение на два цикла, но так компактнее
        }

        public IVector SolveU(IVector x, bool UseDiagonal)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                //проверка корректности
                if (this.Size != x.Size)
                    throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции SolveU");
                else
                {
                    IVector result = (IVector)x.Clone();

                    for (int i = Size - 1; i >= 0; i--)
                    {
                        if (UseDiagonal == true)
                            result[i] /= DU[i];
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            result[jg[j]] -= U[j] * result[i];
                    }
                    return result;
                }
            }
            else
                throw new CannotMultException("Ошибка. Невозможно выполнить функцию SolveU, т.к. не удалось сделать разложение LU.");
        }


        public IVector MultL(IVector x, bool UseDiagonal)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                if (this.Size != x.Size)
                    throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции MultL");
                else
                {
                    IVector result = new SimpleVector(this.Size);
                    for (int i = 0; i < Size; i++)
                    {
                        if (UseDiagonal == true)
                            result[i] = DL[i] * x[i];
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            result[i] += L[j] * x[jg[j]];
                    }
                    return result;
                }
            }
            else
                throw new CannotMultException("Ошибка. Невозможно выполнить функцию MultL, т.к. не удалось сделать разложение LU.");

        }

        public IVector MultU(IVector x, bool UseDiagonal)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                if (this.Size != x.Size)
                    throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции MultU");
                else
                {
                    IVector result = new SimpleVector(Size);
                    for (int i = 0; i < Size; i++)
                    {
                        if (UseDiagonal == true)
                            result[i] = DU[i] * x[i];
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            result[jg[j]] += U[j] * x[i];
                    }

                    return result;
                }
            }
            else
                throw new CannotMultException("Ошибка. Невозможно выполнить функцию MultU, т.к. не удалось сделать разложение LU.");
          
        }

        protected IVector MultT(IVector x,bool UseDiagonal)
        {
            int j;
            IVector result = new SimpleVector(Size);
            if (UseDiagonal)
            {
                for (int i = 0; i < this.Size; i++)
                    result[i] = di[i] * x[i];
            }
            for (int i = 0; i < this.Size; i++)
            {
                {
                    for (int k = ig[i]; k < ig[i + 1]; k++)
                    {
                        j = jg[k];
                        result[j] += al[k] * x[i];
                        result[i] += au[k] * x[j];
                    }
                }
            }
            return result;
        }

        protected IVector SolveLT(IVector x, bool UseDiagonal)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                //проверка корректности
                if (this.Size != x.Size)
                    throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции SolveLT");
                else
                {
                    IVector result = (IVector)x.Clone();

                    for (int i = Size - 1; i >= 0; i--)
                    {
                        if (UseDiagonal == true)
                            result[i] /= DL[i];
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            result[jg[j]] -= L[j] * result[i];
                    }
                    return result;
                }
            }
            else
                throw new CannotMultException("Ошибка. Невозможно выполнить функцию SolveLT, т.к. не удалось сделать разложение LU.");

        }

        protected IVector SolveUT(IVector x, bool UseDiagonal)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                //проверка корректности
                if (this.Size != x.Size)
                    throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции SolveUT");
                else
                {
                    IVector result = new SimpleVector(this.Size);
                    double sum;

                    for (int i = 0; i < this.Size; i++)
                    {
                        sum = 0;
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            sum += U[j] * result[jg[j]];
                        result[i] = (x[i] - sum);
                        if (UseDiagonal == true)
                            result[i] /= DU[i];
                    }
                    return result;
                }
            }
            else
                throw new CannotMultException("Ошибка. Невозможно выполнить функцию SolveUT, т.к. не удалось сделать разложение LU.");

        }

        protected IVector MultLT(IVector x, bool UseDiagonal)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                if (this.Size != x.Size)
                    throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции MultLT");
                else
                {
                    IVector result = new SimpleVector(Size);
                    for (int i = 0; i < Size; i++)
                    {
                        if (UseDiagonal == true)
                            result[i] = DL[i] * x[i];
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            result[jg[j]] += L[j] * x[i];
                    }

                    return result;
                }
            }
            else
                throw new CannotMultException("Ошибка. Невозможно выполнить функцию MultLT, т.к. не удалось сделать разложение LU.");

        }

        protected IVector MultUT(IVector x, bool UseDiagonal)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                if (this.Size != x.Size)
                    throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции MultUT");
                else
                {
                    IVector result = new SimpleVector(this.Size);
                    for (int i = 0; i < Size; i++)
                    {
                        if (UseDiagonal == true)
                            result[i] = DU[i] * x[i];
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            result[i] += U[j] * x[jg[j]];
                    }
                    return result;
                }
            }
            else
                throw new CannotMultException("Ошибка. Невозможно выполнить функцию MultUT, т.к. не удалось сделать разложение LU.");

        }
        public IVector SolveD(IVector x)
        {
            if (this.Size != x.Size)
                throw new Exception("Ошибка. Различие в размерности вектора и матрицы в функции SolveL");
            IVector result = new SimpleVector(Size);
            for (int i = 0; i < Size; i++)
                result[i] = x[i] / di[i];
            return result;
        }

        public void MakeLUSeidel()
        {
            throw new NotImplementedException();
        }

        public static void localtest()
        {
            double[] di = new double[5] { 1, 2, 3, 4, 5 };
            double[] au = new double[4] { 2, 1, 4, 5 };
            double[] al = new double[4] { 3, 4, 5, 6 };
            int[] ig = new int[6] { 0, 0, 0, 1, 2, 4 };
            int[] jg = new int[4] { 0, 1, 0, 1 };

            IMatrix matr = new SparseRowColumnMatrix(ig, jg, di, al, au);

            double res1 = matr[0 ,0]; //1
            double res2 = matr[3, 1]; //4
            double res3 = matr[1, 4]; //5
            double res4 = matr[4, 3]; //0

            IVector x = new SimpleVector(new double[5] { 1, 2, 3, 4, 5 });

            // 27, 33, 12, 24, 42
            IVector y = matr.Mult(x);
            IVector z = new SimpleVector(new double[5] { 27, 33, 72, 74, 84 });
            IVector a = (IVector)y.Clone();

            z = matr.SolveL(z);
            a = matr.SolveU(z);
            // 1,2,3,4,5

            // 35,50,11,18,39
            y = matr.T.Mult(x);
            // 1,2,5,5,14
            y = matr.T.MultU(x);

            IMatrix matr2 = new SparseRowColumnMatrix(new int[4] { 0, 0, 1, 3 }, new int[3] { 0, 0, 1 }, new double[3] { 1, 2, 3 }, new double[3] { 4, 5, 6 },  new double[3] { 4, 5, 6 });

            IVector x2 = new SimpleVector(new double[3] { 1, 2, 3 });
            IVector y2 = (IVector)x2.Clone();
            y2 = matr2.T.SolveU(x2);
            y2 = matr2.T.SolveL(y2);
            // 3/7 1/7 0
        }

        public object Clone()
        {
            return new SparseRowColumnMatrix(this.ig, this.jg, this.di, this.al, this.au);
        }
    }
}
