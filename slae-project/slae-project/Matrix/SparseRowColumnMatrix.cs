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
            public void MakeLUSeidel() => Matrix.MakeLUSeidel();
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
        private double[] D;
        public double this[int i, int j]
        {
            get
            {
                bool down = true;
                if (i == j)
                    return di[i];
                if (i < j)
                {
                    i = i + j; j = i - j; i = i - j; down = false;
                }
                for (int k = ig[i]; k < ig[i + 1]; k++)
                    if (jg[k] == j)
                        if (down) return au[k];
                        else return al[k];
                return 0;
            }
            set
            {
                bool down = true;
                if (i == j)
                    di[i] = value;
                if (i < j)
                {
                    i = i + j; j = i - j; i = i - j; down = false;
                }
                for (int k = ig[i]; k < ig[i + 1]; k++)
                    if (jg[k] == j)
                        if (down) au[k] = value;
                        else al[k] = value;
            }
        }
        // размерность вектора di, =ig[last]
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

            //дописать обход, зачем он вообще нужен?
        public IEnumerator<(double value, int row, int col)> GetEnumerator()
        {
            throw new NotImplementedException();
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
            
            L = al;
            U = au;
            
            D = di;

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
                    U[m] = (U[m] - sum_u) / D[jg[m]];
                    if (double.IsInfinity(U[m]))
                        throw new Exception("Ошибка при LU предобуславливании!");
                    sum_d += L[m] * U[m];
                }
                D[k1] = L[k1] - sum_d;
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
                    throw new Exception("Ошибка. Различие в размерности вектора и матрицы в функции SolveL");
                else
                {
                    IVector result = new SimpleVector(this.Size);
                    double sum;

                    for (int i = 0; i < this.Size; i++)
                    {
                        sum = 0;
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            sum += al[j] * result[jg[j]];
                        result[i] = (x[i] - sum);
                        if (UseDiagonal == true)
                            result[i] /= di[i];
                    }
                    return result;
                }
            }
            else
                throw new Exception("Ошибка. Невозможно выполнить функцию SolveL, т.к. не удалось сделать разложение LU.");

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
                    throw new Exception("Ошибка. Различие в размерности вектора и матрицы в функции SolveU");
                else
                {
                    IVector result = (IVector)x.Clone();

                    for (int i = Size - 1; i >= 0; i--)
                    {
                        if (UseDiagonal == true)
                            result[i] /= di[i];
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            result[jg[j]] -= au[j] * result[i];
                    }
                    return result;
                }
            }
            else
                throw new Exception("Ошибка. Невозможно выполнить функцию SolveU, т.к. не удалось сделать разложение LU.");
        }


        public IVector MultL(IVector x, bool UseDiagonal)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                if (this.Size != x.Size)
                    throw new Exception("Ошибка. Различие в размерности вектора и матрицы в функции MultL");
                else
                {
                    IVector result = new SimpleVector(this.Size);
                    for (int i = 0; i < Size; i++)
                    {
                        if (UseDiagonal == true)
                            result[i] = di[i] * x[i];
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            result[i] += al[j] * x[jg[j]];
                    }
                    return result;
                }
            }
            else
                throw new Exception("Ошибка. Невозможно выполнить функцию MultL, т.к. не удалось сделать разложение LU.");

        }

        public IVector MultU(IVector x, bool UseDiagonal)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                if (this.Size != x.Size)
                    throw new Exception("Ошибка. Различие в размерности вектора и матрицы в функции MultU");
                else
                {
                    IVector result = new SimpleVector(Size);
                    for (int i = 0; i < Size; i++)
                    {
                        if (UseDiagonal == true)
                            result[i] = di[i] * x[i];
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            result[jg[j]] += au[j] * x[i];
                    }

                    return result;
                }
            }
            else
                throw new Exception("Ошибка. Невозможно выполнить функцию MultU, т.к. не удалось сделать разложение LU.");
          
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
                    throw new Exception("Ошибка. Различие в размерности вектора и матрицы в функции SolveLT");
                else
                {
                    IVector result = (IVector)x.Clone();

                    for (int i = Size - 1; i >= 0; i--)
                    {
                        if (UseDiagonal == true)
                            result[i] /= di[i];
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            result[jg[j]] -= al[j] * result[i];
                    }
                    return result;
                }
            }
            else
                throw new Exception("Ошибка. Невозможно выполнить функцию SolveLT, т.к. не удалось сделать разложение LU.");

        }

        protected IVector SolveUT(IVector x, bool UseDiagonal)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                //проверка корректности
                if (this.Size != x.Size)
                    throw new Exception("Ошибка. Различие в размерности вектора и матрицы в функции SolveUT");
                else
                {
                    IVector result = new SimpleVector(this.Size);
                    double sum;

                    for (int i = 0; i < this.Size; i++)
                    {
                        sum = 0;
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            sum += au[j] * result[jg[j]];
                        result[i] = (x[i] - sum);
                        if (UseDiagonal == true)
                            result[i] /= di[i];
                    }
                    return result;
                }
            }
            else
                throw new Exception("Ошибка. Невозможно выполнить функцию SolveUT, т.к. не удалось сделать разложение LU.");

        }

        protected IVector MultLT(IVector x, bool UseDiagonal)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                if (this.Size != x.Size)
                    throw new Exception("Ошибка. Различие в размерности вектора и матрицы в функции MultLT");
                else
                {
                    IVector result = new SimpleVector(Size);
                    for (int i = 0; i < Size; i++)
                    {
                        if (UseDiagonal == true)
                            result[i] = di[i] * x[i];
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            result[jg[j]] += al[j] * x[i];
                    }

                    return result;
                }
            }
            else
                throw new Exception("Ошибка. Невозможно выполнить функцию MultLT, т.к. не удалось сделать разложение LU.");

        }

        protected IVector MultUT(IVector x, bool UseDiagonal)
        {
            if (!LU_was_made)
                MakeLU();
            if (LU_was_made)
            {
                if (this.Size != x.Size)
                    throw new Exception("Ошибка. Различие в размерности вектора и матрицы в функции MultUT");
                else
                {
                    IVector result = new SimpleVector(this.Size);
                    for (int i = 0; i < Size; i++)
                    {
                        if (UseDiagonal == true)
                            result[i] = di[i] * x[i];
                        for (int j = ig[i]; j < ig[i + 1]; j++)
                            result[i] += au[j] * x[jg[j]];
                    }
                    return result;
                }
            }
            else
                throw new Exception("Ошибка. Невозможно выполнить функцию MultUT, т.к. не удалось сделать разложение LU.");

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
    }
}
