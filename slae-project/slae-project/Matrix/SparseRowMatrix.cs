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

        int[] ig;
        int[] jg;
        double[] al; //если симметричная, то нижний треугольник и диагональ, иначе все элементы
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
                IVector diag = new SimpleVector(Size);
                for (int i = 0; i < Size; i++)
                    //if (SearchPlaceInAl(i,i,out k))
                        diag[i] = this[i,i];
                return diag;
            }
        }
        
       public static Dictionary<string, string> requiredFileNames => new Dictionary<string, string>
        {
            { "ig", "Файл состоит из одной строки: "+
                " элементов массива (integer), разделенные пробелом." },
            { "jg", "Файл состоит из одной строки: "+
                "элементов массива (integer), разделенные пробелом" },
            {"al", "Файл состоит из одной строки: "+
                "элементы матрицы (double), разделенные пробелом" },

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
            this.ig = ig.Clone() as int[];
            this.jg = jg.Clone() as int[];
            this.al = al.Clone() as double[];
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
                List<int> tempig = new List<int>(ig);
                double ElemNew;
                for (int i = 0; i < Size; i++)
                    for (int j = i+1; j < Size; j++)
                    {
                        ElemNew = this[j, i];
                        if (ElemNew != 0)
                        {
                            tempal.Insert(tempig[i + 1], ElemNew);
                            tempjg.Insert(tempig[i + 1], j);
                            for(int k=i+1;k<=Size;k++)
                                tempig[k]++;
                        }
                    }
                al = tempal.ToArray();
                jg = tempjg.ToArray();
                ig = tempig.ToArray();
                isSymmetric = false;
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
                result = this.SolveLT(x, UseDiagonal);
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
                for (int i = Size - 1; i >= 0; i--)
                {
                    for (int j = 0; j < i; j++)
                    {
                        result[i] -= this[j, i] * result[j];
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
                for (int i = 0; i < this.Size; i++)
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
            Dictionary<string, string> pathFiles = new Dictionary<string, string>
        {
            { "ig", "./ig.txt" },
            { "jg", "./jg.txt"},
            {"al", "./al.txt" },

        };
            double[] vecval = new double[3] { 3, 2, 2};
            SparseRowMatrix s_matr = new SparseRowMatrix(pathFiles);
            s_matr.CastToNotSymm();
            IVector newV = new SimpleVector(vecval);
            IVector mult = s_matr.MultL(newV,true);
            IVector multTT = s_matr.MultU(newV,true);
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
                    case "ig":
                        var reader = new StreamReader(el.Value);

                        //считывание элементов массива
                        line = reader.ReadLine();
                        sub = line.Split(' ', '\t');

                        
                        n = sub.Length - 1;
                        ig = new int[n + 1];
                        for (int i = 0; i < n+1; i++)
                                ig[i] = Convert.ToInt32(sub[i]);
                        
                        if (jg != null || al != null )
                            if (ig[n + 1] != m)
                                throw new CannotFillMatrixException("Ошибка при считывании файла ig. Массив не соответсвует другим массивам. Проверьте файлы al, jg");
                        m = ig[n];
                        count_files++;
                        reader.Close();
                        break;
                    case "jg":
                        reader = new StreamReader(el.Value);
                        //считывание элементов массива
                        line = reader.ReadLine();
                        sub = line.Split(' ', '\t');
                        if (al != null||ig!=null)
                        {
                            if (m != sub.Length)
                                throw new CannotFillMatrixException("Ошибка при считывании файла jg. Массив не соответсвует другим массивам.");
                        }
                        else
                            m = sub.Length;
                        jg = new int[m];
                        for (int i = 0; i < m; i++)
                                jg[i] = Convert.ToInt32(sub[i]);
                        
                        count_files++;
                        reader.Close();
                        break;
                  
                    case "al":
                        reader = new StreamReader(el.Value);
                        //считывание элементов массива
                        line = reader.ReadLine();
                        sub = line.Split(' ', '\t');

                        // проверяем корректность
                        if (ig != null || jg != null)
                        {
                            if (sub.Length != m)
                                throw new CannotFillMatrixException("Ошибка при считывании файла al. Массив не соответствует другим массивам.");
                        }
                        else
                            m = sub.Length;
                        al = new double[m];

                        for (int i = 0; i < m; i++)
                            al[i] = Convert.ToDouble(sub[i]);

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
