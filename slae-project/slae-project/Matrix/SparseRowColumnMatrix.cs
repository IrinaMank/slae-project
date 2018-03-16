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

        //bool LU_was_made = false;
        //// портрет сохраняется
        //private double[] L;
        //private double[] U;
        //private double[] DL;
        //// диагональ для матрицы U
        //private double[] DU;
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

        public List<string> requiredFileNames => throw new NotImplementedException();

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
            
            List <List <KeyValuePair<int, double>>> elements_al;
            List <List <KeyValuePair<int, double>>> elements_au;

            elements_al = new List<List<KeyValuePair<int, double>>>();
            elements_au = new List<List<KeyValuePair<int, double>>>();
            

            this.Size = c_matrix.Size;
            
            for (int m=0; m< this.Size; m++)
            {
                elements_al.Add(new List<KeyValuePair<int, double>>());
                elements_au.Add(new List<KeyValuePair<int, double>>());
            }

            di = new double[this.Size];
            ig = new int[this.Size + 1];

            int i, j;
            foreach (var val in c_matrix)
            {
                i = val.row;
                j = val.col;
                if (i == j)
                    di[i] = val.value;
                else
                {
                    if (i > j)
                        elements_al[i].Add(new KeyValuePair<int, double>(j, val.value));
                    else
                        elements_au[j].Add(new KeyValuePair<int, double>(i, val.value));
                }
            }

            ig[0] = 0; ig[1] = 0;
            for (int k=2; k<=this.Size; k++)
            {
                if (elements_al[k-1].Count == elements_au[k-1].Count)
                {
                    ig[k] = ig[k-1]+elements_al[k - 1].Count;
                }
                else
                {
                    if(elements_al[k-1].Count < elements_au[k-1].Count)
                        ig[k] = ig[k - 1] + elements_au[k - 1].Count;
                    else
                        ig[k] = ig[k - 1] + elements_al[k - 1].Count;
                }
            }

            al = new double[ig[this.Size]];
            au = new double[ig[this.Size]];
            jg = new int[ig[this.Size]];
            int tmp = 0;
            for (int k = 0; k < this.Size; k++)
            {
                for (int l = 0; l < elements_al[k].Count || l < elements_au[k].Count; l++, tmp++)
                {
                    if (elements_al[k].Count>0 && 
                        elements_au[k].Count>0 &&
                        l < elements_al[k].Count &&
                        l < elements_au[k].Count)
                        if (elements_al[k][l].Key == elements_au[k][l].Key)
                        {
                            al[tmp] = elements_al[k][l].Value;
                            au[tmp] = elements_au[k][l].Value;
                            jg[tmp] = elements_au[k][l].Key;
                        }
                        else
                        {
                            if (elements_al[k][l].Key < elements_au[k][l].Key)
                            {
                                al[tmp] = elements_al[k][l].Value;
                                au[tmp] = 0;
                                jg[tmp] = elements_al[k][l].Key;
                            }
                            else
                            {
                                al[tmp] = 0;
                                au[tmp] = elements_au[k][l].Value;
                                jg[tmp] = elements_au[k][l].Key;
                            }
                        }
                    else
                    {
                        if (l < elements_au[k].Count)
                        {
                            if (elements_al[k].Count == 0)
                                al[tmp] = 0;
                            else
                                for (int h = 0; h < elements_al[k].Count; h++)
                                {
                                    if (elements_al[k][h].Key == elements_au[k][l].Key)
                                    { al[tmp] = elements_al[k][h].Value; break; }
                                }
                            au[tmp] = elements_au[k][l].Value;
                            jg[tmp] = elements_au[k][l].Key;
                        }
                        else
                        {
                            al[tmp] = elements_al[k][l].Value;
                            if (elements_au[k].Count == 0)
                                au[tmp] = 0;
                            else
                                for (int h = 0; h < elements_al[k].Count; h++)
                                {
                                    if (elements_al[k][l].Key == elements_au[k][h].Key)
                                    { au[tmp] = elements_au[k][h].Value; break; }
                                }
                            
                            jg[tmp] = elements_al[k][l].Key;
                            
                          
                        }
                    }

                }
            }
            
        }

        public SparseRowColumnMatrix()
        {
        }

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
                                sum_l += al[i] * au[j];
                                sum_u += al[j] * au[i];
                                j_s++;
                            }
                        }
                    }
                    al[m] = al[m] - sum_l;
                    au[m] = (au[m] - sum_u) / di[jg[m]];
                    if (double.IsInfinity(au[m]))
                        throw new LUFailException("Ошибка при LU предобуславливании!");
                    sum_d += al[m] * au[m];
                }
                di[k1] = di[k1] - sum_d;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="UseDiagonal">различие в делении на диагональ</param>
        /// <returns></returns>
        public IVector SolveL(IVector x, bool UseDiagonal)
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
                        sum += al[j] * result[jg[j]];
                    result[i] = (x[i] - sum);
                    if (UseDiagonal == true)
                        result[i] /= di[i];
                }
                return result;
            }
            
            // можно было сделать главное условие if(UseDiagonal==true) и разбиение на два цикла, но так компактнее
        }

        public IVector SolveU(IVector x, bool UseDiagonal)
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
                        result[i] /= di[i];
                    for (int j = ig[i]; j < ig[i + 1]; j++)
                        result[jg[j]] -= au[j] * result[i];
                }
                return result;
            }
            
        }


        public IVector MultL(IVector x, bool UseDiagonal)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции MultL");
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

        public IVector MultU(IVector x, bool UseDiagonal)
        {
            if (this.Size != x.Size)
                    throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции MultU");
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
            //проверка корректности
            if (this.Size != x.Size)
                throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции SolveLT");
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

        protected IVector SolveUT(IVector x, bool UseDiagonal)
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
                        sum += au[j] * result[jg[j]];
                    result[i] = (x[i] - sum);
                    if (UseDiagonal == true)
                        result[i] /= di[i];
                }
                return result;
            }
            
        }

        protected IVector MultLT(IVector x, bool UseDiagonal)
        {
            if (this.Size != x.Size)
                    throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции MultLT");
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

        protected IVector MultUT(IVector x, bool UseDiagonal)
        {
            if (this.Size != x.Size)
                    throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции MultUT");
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
            (int, int)[] coord = new(int, int)[11];
            coord[0] = (0, 0);
            coord[1] = (0, 2);
            coord[2] = (0, 3);
            coord[3] = (1, 1);
            coord[4] = (1, 3);
            coord[5] = (2, 2);
            coord[6] = (3, 1);
            coord[7] = (3, 3);
            coord[8] = (4, 1);
            coord[9] = (4, 2);
            coord[10] = (4, 4);

            double[] val = new double[11] { 1,3,4,2,5,3,1,4,2,3,5 };

            IMatrix c_matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)c_matr);


        }

        public object Clone()
        {
            return new SparseRowColumnMatrix(this.ig, this.jg, this.di, this.al, this.au);
        }

        public void FillByFiles(Dictionary<string, string> paths)
        {
            throw new NotImplementedException();
        }
    }
}
