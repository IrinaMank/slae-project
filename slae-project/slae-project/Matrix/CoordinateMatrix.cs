using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Matrix
{
    class CoordinateMatrix : IMatrix
    {
        public int n;
        bool LU_maked;
        struct element
        {
            public double val;
            public int i;
            public int j;

            public element(double val, int i, int j)
            {
                this.val = val;
                this.i = i;
                this.j = j;
            }
        };
        List<element> elements;
        List<double[]> L;
        List<double[]> U;
        // Индексер класса
        public double this[int i, int j]
        {
            get
            {
                foreach (element el in elements)
                    if (el.i == i && el.j == j)
                        return el.val;
                return 0;
            }
            set
            {
                if (value != 0)
                {
                    for (int k = 0; k < elements.Count; k++)
                    {
                        element temp = elements[k];
                        if (temp.i == i && temp.j == j)
                        {
                            elements.Remove(temp);
                            break;
                        }
                    }
                    elements.Add(new element(value, i, j));
                    LU_maked = false;
                }
            }
        }

        // Конструктор
        public CoordinateMatrix(int n)
        {
            LU_maked = false;
            this.n = n;
        }

        // Стандартная опреация умножения матрицы на вектор
        // inverse = true предполагает умножение обратной матрицы вместо исходной
        public Vector Mult(Vector vec, bool inverse = false)
        {
            if (this.n != vec.size)
                return null;
            Vector result = new Vector(n);
            foreach (element el in elements)
            {
                result[el.i] += el.val * vec[el.j];
            }
            return result;
        }

        // Создание LU-разложения
        void MakeLU()
        {
            //Выделение памяти
            L = new List<double []> { };
            U = new List<double[]> { };
            for (int i = 1; i <= n; i++)
            {
                L.Add(new double[i]);
                U.Add(new double[i]);
            }
            // Разложение
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    U[0][i] = this[0, i];
                    L[i][0] = this[i, 0] / U[0][0];
                    double sum = 0;
                    for (int k = 0; k < i; k++)
                    {
                        sum += L[i][k] * U[k][j];
                    }
                    U[i][j] = this[i, j] - sum;
                    if (i > j)
                    {
                        L[j][i] = 0;
                    }
                    else
                    {
                        sum = 0;
                        for (int k = 0; k < i; k++)
                        {
                            sum += L[j][k] * U[k][i];
                        }
                        L[j][i] = (this[j, i] - sum) / U[i][i];
                    }
                }
            }
            LU_maked = true;
        }
        // Операция умножения L-компоненты LU-разложения матрицы на вектор
        // inverse = true предполагает умножение обратной матрицы вместо исходной
        public Vector MultL(Vector vec, bool inverse = false)
        {
            if (!LU_maked)
                MakeLU();
            return null;
        }

        // Операция умножения U-компоненты LU-разложения матрицы на вектор
        // inverse = true предполагает умножение обратной матрицы вместо исходной
        public Vector MultU(Vector vec, bool inverse = false)
        {
            if (!LU_maked)
                MakeLU();
            return null;
        }
    }
}
