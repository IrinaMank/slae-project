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
using System.IO;

namespace slae_project.Matrix
{
    public class SparseRowMatrix : IMatrix
    {
        //что делать с транспонированием?
        public class TransposeIllusion : ILinearOperator
        {
            public SparseRowMatrix Matrix { get; set; }
            public ILinearOperator Transpose => Matrix;
            public ILinearOperator T => Matrix;
            public IVector Diagonal { get; }
            public int Size => Matrix.Size;
            public IVector MultL(IVector x, bool UseDiagonal) => Matrix.MultLT(x, UseDiagonal);
            public IVector SolveL(IVector x, bool UseDiagonal) => Matrix.SolveLT(x, UseDiagonal);
            public IVector Mult(IVector x, bool UseDiagonal) => Matrix.MultT(x, UseDiagonal);
            public IVector MultU(IVector x, bool UseDiagonal) => Matrix.MultUT(x, UseDiagonal);
            public IVector SolveU(IVector x, bool UseDiagonal) => Matrix.SolveUT(x, UseDiagonal);
            public IVector SolveD(IVector x) => Matrix.SolveD(x);
            public void MakeLU() => Matrix.MakeLU();
            public object Clone() => Matrix.Clone();

            public IVector MultD(IVector a)
            {
                throw new NotImplementedException();
            }
        }


        //необходимые вектора для реализации строчно-столбцового формата
        int[] ig;
        int[] jg;
        double[] al;
        double extraDiagVal = 0;
        bool isSymmetric;

        public double this[int i, int j]
        {
            get
            {
                if (this.isSymmetric)
                {
                    if (i < j)
                    {
                        int f = i; i = j; j = f;
                    }
                    if (SearchPlaceInAl(i, j, out int k))
                        return al[k];
                    else return 0;

                }
                else
                {
                    if (SearchPlaceInAl(i, j, out int k))
                        return al[k];
                    else return 0;
                }
            }
            set
            {
                if (SearchPlaceInAl(i, j, out int k))
                    al[k] = value;
            }
        }

        private bool SearchPlaceInAl(int i, int j,  out int last)
        {
            // бинарный поиск
            if(this.isSymmetric && i<j)
            {
                int k = i; i = j; j = k;
            }
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

        public int Size { get; private set;  }

        public ILinearOperator Transpose => new TransposeIllusion { Matrix = this };
        public ILinearOperator T => new TransposeIllusion { Matrix = this };

        public IVector Diagonal
        {
            get
            {
                int k;
                IVector diag = new SimpleVector(0);
                for (int i = 0; i < Size; i++)
                    if (SearchPlaceInAl(i,i,out k))
                        diag[i] = k;
                return diag;
            }
        }
        
       public static Dictionary<string, string> requiredFileNames => new Dictionary<string, string>
        {
            { "ig", "Файл состоит из двух строк: количество элементов массива ig"+
                " (integer) и элементов массива (integer), разделенных пробелом." },
            { "jg", "Файл состоит из двух строк: количество элементов массива jg - "+
                "количество ненулевых недиагональных элементов"+
                " (integer) и элементов массива (integer), разделенных пробелом" },
            {"al", "Файл состоит из двух строк: количество ненулевых элементов  матрицы "+
                "(integer) и элементы матрицы (double), разделенных пробелом" },

        };
       

        public IEnumerator<(double value, int row, int col)> GetEnumerator()
        {
            IEnumerable<(double value, int row, int col)> F()
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = ig[i]; j < ig[i + 1]; j++)
                    {
                        yield return (al[j], i, jg[j]);
                        if(this.isSymmetric) yield return (al[j], jg[j], i);
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
        /// Инициализация матрицы массивами ig,jg,au
        /// </summary>
        /// <param name="ig">Индексы начала строк</param>
        /// <param name="jg">Номера столбцов для элементов</param>
        /// <param name="al">Элементы нижней диагонали</param>
        /// <param name="isSym">Симметричность матрицы</param>
        public SparseRowMatrix(int[] ig, int[] jg, double[] al, bool isSym=false)
        {
            this.ig = ig;
            this.jg = jg;
            this.al = al;
            this.Size = ig.Length-1;
            this.isSymmetric = isSym;
        }

        public SparseRowMatrix(int n, bool isSym = false)
        {
            this.isSymmetric = isSym;
            ig = new int[n + 1];

            for(int i=0; i<n; i++)
            {
                ig[i] = 0; //в строках нет элементов
            }
            //остальные массивы не создаются, т.к. они пустые
        }

        public SparseRowMatrix(CoordinateMatrix c_matrix,bool isSym = false)
        {
            this.isSymmetric = isSym;
            List <List <KeyValuePair<int, double>>> elements_al;
            elements_al = new List<List<KeyValuePair<int, double>>>();

            this.Size = c_matrix.Size;
            
            for (int m=0; m< this.Size; m++)
            {
                elements_al.Add(new List<KeyValuePair<int, double>>());
            }

            ig = new int[this.Size + 1];

            int i, j;
            foreach (var val in c_matrix)
            {
                i = val.row;
                j = val.col;
                elements_al[i].Add(new KeyValuePair<int, double>(j, val.value));
            }

            ig[0] = 0; 
            for (int k=1; k<=this.Size; k++)
            {
                    ig[k] = ig[k-1]+elements_al[k - 1].Count;
            }

            al = new double[ig[this.Size]];
            jg = new int[ig[this.Size]];
            int tmp = 0;
            for (int k = 0; k < this.Size; k++)
            {
                for (int l = 0; l < elements_al[k].Count; l++, tmp++)
                {
                        al[tmp] = elements_al[k][l].Value;
                        jg[tmp] = elements_al[k][l].Key;
                }
            }
            
        }

        public SparseRowMatrix(bool isSym=false)
        {
            this.Size = 0;
            this.isSymmetric = isSym;
        }

        public IVector Mult(IVector x, bool UseDiagonal)
        {
            if (this.Size != x.Size)
                throw new Exception("Ошибка. Различие в размерности вектора и матрицы в функции Mult");
            int j;
            IVector result = new SimpleVector(Size);
            if (isSymmetric)
            {
                for (int i = 0; i < this.Size; i++)
                    for (int k = ig[i]; k < ig[i + 1] - 1; k++)
                    {
                        if (jg[k] != i)
                        {
                            j = jg[k];
                            result[i] += al[k] * x[j];
                            result[j] += al[k] * x[i];
                        }
                        else
                            if (UseDiagonal)
                            result[i] += al[k] * x[jg[k]];
                    }
            }
            else
            {
                for (int i = 0; i < this.Size; i++)
                    for (int k = ig[i]; k < ig[i + 1]; k++)
                    {
                        j = jg[k];
                        if (j != i)
                        {
                            result[i] += al[k] * x[j];
                        }
                        else
                            if (UseDiagonal)
                            result[i] += al[k] * x[j];
                    }
            }
            return result;
        }
        private void CastToNotSymm()
        {
            if (isSymmetric)
            {
                List<double> tempal= new List<double>(al);
                List<int> tempjg = new List<int>(jg);
                isSymmetric = false;
                double ElemNew;
                for (int i = 0; i < Size; i++)
                    for (int j = 0; j < i; j++)
                    {
                        ElemNew = this[i, j];
                        if (ElemNew != 0)
                        {
                            tempal.Insert(ig[i + 1], ElemNew);
                            tempjg.Insert(ig[i + 1], j);
                            ig[i + 1]++;
                        }
                    }
                al = tempal.ToArray();
                jg = tempjg.ToArray();
            }
        }
        /// <summary>
        /// Создание LU разложения
        /// </summary>
        public void MakeLU()
        {
            try
            {
                CastToNotSymm();
                double sum;
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        sum = 0;
                        if (this[i, j] != 0 && i >= j)
                        {
                            for (int k = 0; k < i; k++)
                            {
                                sum += this[i, k] * this[k, j];
                            }
                            this[i, j] = this[i, j] - sum;
                        }
                        if (this[j, i] != 0 && i < j)
                        {
                            sum = 0;
                            for (int k = 0; k < i; k++)
                            {
                                sum += this[j, k] * this[k, i];
                            }
                            if (Math.Abs(this[i, j]) < 1e-10)
                                throw new DivideByZeroException();
                            this[j, i] = (this[j, i] - sum) / this[i, i];
                        }
                    }
                }
                extraDiagVal = 1;
            }
            catch (DivideByZeroException)
            {
                throw new LUFailException("Невозможгно выполнить LU разложение");
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
                int jCol;
                var di = this.Diagonal;
                for (int i = 0; i < this.Size; i++)
                {
                    sum = 0;
                    for (int j = ig[i]; j < ig[i + 1]; j++)
                    {
                        jCol = jg[j];
                        if (jCol < i)
                            sum += al[j] * result[jCol];
                        else
                            break;
                    }
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
            IVector result = (IVector)x.Clone();
            int jCol;
            var di = this.Diagonal;
            if (this.isSymmetric)
                result=this.SolveLT(x,UseDiagonal);
            else
                for (int i = Size - 1; i >= 0; i--)
                {

                    for (int j = ig[i + 1] - 1; j >= ig[i]; j--)
                    {
                        jCol = jg[j];
                        if (jCol > i)
                            result[i] -= al[j] * result[jCol];
                        else
                            break;
                    }
                    if (UseDiagonal == true && extraDiagVal == 0)
                    {
                        result[i] /= di[i];
                    }
                }
            return result;
        }

        public IVector MultL(IVector x, bool UseDiagonal)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции MultL");
            else
            {
                int jCol;
                var di = this.Diagonal;
                IVector result = new SimpleVector(this.Size);
                for (int i = 0; i < Size; i++)
                {
                    if (UseDiagonal == true)
                        result[i] = di[i] * x[i];
                    for (int j = ig[i]; j < ig[i + 1]; j++)
                    {
                        jCol = jg[j];
                        if (jCol < i)
                           result[i] += al[j] * x[jCol];
                        else
                           break;
                    }
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
                int jCol;
                IVector result = new SimpleVector(Size);
                if (this.isSymmetric)
                    result = this.MultLT(x, UseDiagonal);
                else
                    for (int i = 0; i < Size; i++)
                    {
                        if (UseDiagonal == true)
                        {
                            var di = this.Diagonal;
                            if (extraDiagVal == 0)
                                result[i] = di[i] * x[i];
                            else
                                result[i] = x[i];
                        }
                        for (int j = ig[i + 1] - 1; j >= ig[i]; j--)
                        {
                            jCol = jg[j];
                            if (jCol > i)
                                result[i] += al[j] * x[jCol];
                            else
                                break;
                        }
                    }
                return result;
            }
        }

        protected IVector MultT(IVector x,bool UseDiagonal)
        {
            int j;
            if (this.Size != x.Size)
                throw new Exception("Ошибка. Различие в размерности вектора и матрицы в функции Mult");
            IVector result = new SimpleVector(Size);
            if (this.isSymmetric)
                result = Mult(x, UseDiagonal);
            else
                for (int i = 0; i < this.Size; i++)
                {
                    for (int k = ig[i]; k < ig[i + 1]; k++)
                    {
                        j = jg[k];
                        if (j != i)
                        {
                            result[j] += al[k] * x[i];
                        }
                        else
                            if (UseDiagonal)
                            result[i] += al[k] * x[j];
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
                IVector di = this.Diagonal;
                for (int i = 0; i < Size; i--)
                {
                    for (int j = 0; j < i; j++)
                    {
                            result[i] -= this[j,i] * result[j];
                    }
                    if (UseDiagonal == true)
                    {
                        result[i] /= di[i];
                    }
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
                IVector result = (IVector)x.Clone();
                var di = this.Diagonal;
                for (int i = this.Size-1; i >= 0; i--)
                {
                    for (int j = this.Size - 1; j > i; j--)
                    {
                            result[i] -= this[j, i] * result[j];
                    }
                    if (UseDiagonal == true && extraDiagVal == 0)
                            result[i] /= di[i];
                }
                return result;
            }

        }
        /// <summary>
        /// ////////////////////////
        /// </summary>
        /// <param name="x"></param>
        /// <param name="UseDiagonal"></param>
        /// <returns></returns>
        protected IVector MultLT(IVector x, bool UseDiagonal)
        {
            if (this.Size != x.Size)
                    throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции MultLT");
            else
            {
                int jCol;
                IVector result = new SimpleVector(Size);
                for (int i = 0; i < Size; i++)
                {
                    for (int j = ig[i + 1]-1; j >= ig[i]; j--)
                    {
                        jCol = jg[j];
                        if (i == jCol && UseDiagonal)
                        { result[i] += al[i] * x[i]; break; }
                        else
                            result[jCol] += al[j] * x[i];
                    }
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
                int jCol;
                IVector result = new SimpleVector(this.Size);
                for (int i = 0; i < Size; i++)
                {
                    for (int j = ig[i]; j < ig[i + 1]; j++)
                    {
                        jCol = jg[j];
                        if (i == jCol && UseDiagonal)
                        {
                            if (extraDiagVal == 0) result[i] += al[i] * x[i];
                            else
                                result[i] = extraDiagVal * x[i];
                            break;
                        }
                        result[i] += al[j] * x[jCol];
                    }
                }
                return result;
            }
           
        }
        public IVector SolveD(IVector x)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Ошибка. Различие в размерности вектора и матрицы в функции SolveD");
            IVector result = new SimpleVector(Size);
            IVector di = this.Diagonal;
            for (int i = 0; i < Size; i++)
                result[i] = x[i] / di[i];
            return result;
        }

        public static void localtest()
        {
            (int, int)[] coord = new(int, int)[16];
            coord[0] = (0, 0);
            coord[1] = (0, 1);
            coord[2] = (0, 2);
            coord[3] = (0, 3);
            coord[4] = (1, 0);
            coord[5] = (1, 1);
            coord[6] = (1, 2);
            coord[7] = (1, 3);
            coord[8] = (2, 0);
            coord[9] = (2, 1);
            coord[10] = (2, 2);
            coord[11] = (2, 3);
            coord[12] = (3, 0);
            coord[13] = (3, 1);
            coord[14] = (3, 2);
            coord[15] = (3, 3);

            double[] val = new double[16] { 1, 2, 3, 1, 1, 1, 2, 3, 3, 1, 1, 2, 2, 3, 1, 1 };
            double[] vecval = new double[4] { 3, 2, 2, 0 };
            IMatrix c_matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowMatrix((CoordinateMatrix)c_matr);
            IVector newV = new SimpleVector(vecval);
            IVector mult = s_matr.Mult(newV);
            IVector multTT = s_matr.Transpose.Mult(newV);
        }

        public object Clone()
        {
            return new SparseRowMatrix(this.ig, this.jg, this.al);
        }

        public IVector MultD(IVector a)
        {
            IVector di = this.Diagonal;
            IVector result = new SimpleVector(Size);
            for (int i = 0; i < Size; i++)
                result[i] = a[i]*di[i];
            return result;
        }

        public SparseRowMatrix(Dictionary<string, string> paths, bool isSym = false)
        {
            this.isSymmetric = isSym;
            string line;
            string[] sub;
            // n - размерность матрицы, m - количество ненулевых элементов
            int n = 0, m = 0;
            int count_files = 0;
            foreach (var el in paths)
            {
                switch (el.Key)
                {
                    case "ig.txt":
                        var reader = new StreamReader(el.Value);

                        // считываем размерность массива
                        line = reader.ReadLine();
                        sub = line.Split(' ', '\t');
                        n = Convert.ToInt32(sub[0]) - 1;
                        ig = new int[n + 1];

                        //считывание элементов массива
                        line = reader.ReadLine();
                        sub = line.Split(' ', '\t');
                        if (sub.Length != n)
                        {
                            throw new CannotFillMatrixException("Ошибка при считывании файла ig. Некорректная структура файла. Проверьте количество элементов и их фактическое количество");
                        }
                        else
                        {
                            for (int i = 0; i < n + 1; i++)
                                ig[i] = Convert.ToInt32(sub[i]);
                        }
                        if (jg != null || al != null )
                            if (ig[n + 1] != m)
                                throw new CannotFillMatrixException("Ошибка при считывании файла ig. Массив не соответсвует другим массивам. Проверьте файлы al, au, jg");
                        m = ig[n + 1];
                        count_files++;
                        reader.Close();
                        break;
                    case "jg.txt":
                        reader = new StreamReader(el.Value);
                        // считываем размерность массива
                        line = reader.ReadLine();
                        sub = line.Split(' ', '\t');
                        // проверяем корректность
                        if (ig != null || al != null)
                            if (Convert.ToInt32(sub[0]) != m)
                                throw new CannotFillMatrixException("Ошибка при считывании файла jg. Несовпадение размерности массива в соответствии с остальными файлами");
                        jg = new int[m];

                        //считывание элементов массива
                        line = reader.ReadLine();
                        sub = line.Split(' ', '\t');
                        if (sub.Length != m)
                        {
                            throw new CannotFillMatrixException("Ошибка при считывании файла jg. Некорректная структура файла. Проверьте количество элементов и их фактическое количество");
                        }
                        else
                        {
                            for (int i = 0; i < m; i++)
                                jg[i] = Convert.ToInt32(sub[i]);
                        }
                        count_files++;
                        reader.Close();
                        break;
                  
                    case "al.txt":
                        reader = new StreamReader(el.Value);
                        // считываем размерность массива
                        line = reader.ReadLine();
                        sub = line.Split(' ', '\t');

                        // проверяем корректность
                        if (ig != null || jg!= null)
                            if (Convert.ToInt32(sub[0]) != m)
                                throw new CannotFillMatrixException("Ошибка при считывании файла al. Несовпадение размерности массива в соответствии с остальными файлами");
                        al = new double[m];
                        //считывание элементов массива
                        line = reader.ReadLine();
                        sub = line.Split(' ', '\t');
                        if (sub.Length != m)
                        {
                            throw new CannotFillMatrixException("Ошибка при считывании файла al. Некорректная структура файла. Проверьте количество элементов и их фактическое количество");
                        }
                        else
                        {
                            for (int i = 0; i < m; i++)
                                al[i] = Convert.ToDouble(sub[i]);
                        }
                        count_files++;
                        reader.Close();
                        break;
                }
            }
            Size = n;
            if (count_files != 3)
                throw new CannotFillMatrixException("Считаны не все необходимые файлы. Проверьте наличие файлов и их содержимое");
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
    }
}
