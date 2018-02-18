using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * нужен ли класс для создания имитации транспонированной матрицы (скорее всего да)
*/

namespace slae_project.Matrix
{
    class SparseRowColumnMatrix : IMatrix
    {
        //что делать с транспонированием?
        /*class TransposeIllusion : ILinearOperator
        {
            public SparseRowColumnMatrix Matrix { get; set; }

        }*/
        

        //необходимые вектора для реализации строчно-столбцового формата
        List<double> di;
        List<int> ig;
        List<int> jg;
        List<double> al;
        List<double> au;

        bool LU_was_made = false;
        // портрет сохраняется
        private List<double> L;
        private List<double> U;
        // пока выделим диагональ
        private List<double> D;
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
            //TODO: отлавливание неккоректного задания матрицы
            // допустим все корректно
            this.Size = di.Length;
            for (int i = 0; i < Size; i++)
                this.di.Add(di[i]);
            for (int i = 0; i < Size + 1; i++)
                this.ig.Add(ig[i]);
            for (int i = 0; i < ig[Size]; i++)
            {
                this.jg.Add(jg[i]);
                this.al.Add(al[i]);
                this.au.Add(au[i]);
            }
        }

        //Какие еще могут быть виды инициализации, кроме заданных векторов?
        public IVector Mult(IVector x)
        {
            //проверка на корректное умножение (размеры вектора и матрицы)
            //допустим все корректно
            int j;
            IVector result = new SimpleVector(Size);
            for (int i=0; i< this.Size; i++)
            {
                result[i] = di[i] * x[i];
                for (int k=ig[i]; k<ig[i+1]; k++)
                {
                    j = jg[k];
                    result[i] += al[k] * x[j];
                    result[j] += au[k] * x[i];
                }
            }
            return result;
        }

        // нуждается в серьезном тестировании, очень серьезном
        /// <summary>
        /// Создание LU разложения
        /// </summary>
        private void MakeLU()
        {
            // пусть опять же все корректно
            // непонятно что делать с диагональю di, выделять ли ее в разложении
            // если нет, то что с портретом
            L = new List<double> { };
            U = new List<double> { };
            D = new List<double> { };
            double sum_l, sum_u, sum_d;
            
            for (int i = 0; i < ig[Size]; i++)
            {
                L[i] = al[i];
                U[i] = au[i];
            }
            for (int i = 0; i < Size; i++)
                D[i] = di[i];

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
                    sum_d += L[m] * U[m];
                }
                D[k1] = L[k1] - sum_d;
            }
            LU_was_made = true;
        }

        public IVector SolveL(IVector x, bool UseDiagonal)
        {
            
        }

        public IVector SolveU(IVector x, bool UseDiagonal)
        {

        }

        public IVector MultL(IVector x, bool UseDiagonal)
        {

        }

        public IVector MultU (IVector x, bool UseDiagonal)
        {


        }

    }
}
