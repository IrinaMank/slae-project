using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Matrix
{
    class CoordinateMatrix : IMatrix
    {
        /// <summary>
        /// Класс для удобного и быстрого доступа к элементам транспонированной матрицы
        /// без создания таковой
        /// </summary>
        class TransposeIllusion : ILinearOperator
        {
            public CoordinateMatrix Matrix { get; set; }
            public ILinearOperator Transpose => Matrix;
            public ILinearOperator T => Matrix;
            public IVector Diagonal => Matrix.Diagonal;
            public int Size => Matrix.Size;
            public IVector MultL(IVector x, bool UseDiagonal) => Matrix.MultLT(x, UseDiagonal);
            public IVector SolveL(IVector x, bool UseDiagonal) => Matrix.SolveLT(x, UseDiagonal);
            public IVector Mult(IVector x) => Matrix.MultT(x);
            public IVector MultU(IVector x, bool UseDiagonal) => Matrix.MultUT(x, UseDiagonal);
            public IVector SolveU(IVector x, bool UseDiagonal) => Matrix.SolveUT(x, UseDiagonal);
        }
        // Элементы матрицы
        Dictionary<(int i, int j), double> elements = new Dictionary<(int i, int j), double>();
        public double this[int i, int j]
        {
            
            get
            {
                try
                {
                    return elements[(i, j)];
                }
                catch (KeyNotFoundException ex)
                {
                    return 0;
                }
            }
            set => elements[(i, j)] = value;
        }

        // Предполагаются только квадратные матрицы
        public int Size { get; }
        public ILinearOperator Transpose => new TransposeIllusion { Matrix = this };
        public ILinearOperator T => new TransposeIllusion { Matrix = this };

        //TODO: Метод и правда должен что-то возвращать
        public IVector Diagonal => throw new NotImplementedException();

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
        public IVector Mult(IVector x)
        {
            throw new NotImplementedException();
        }
        public IVector SolveL(IVector x, bool UseDiagonal)
        {
            throw new NotImplementedException();
        }
        public IVector SolveU(IVector x, bool UseDiagonal)
        {
            throw new NotImplementedException();
        }
        public IVector MultL(IVector x, bool UseDiagonal)
        {
            throw new NotImplementedException();
        }
        public IVector MultU(IVector x, bool UseDiagonal)
        {
            throw new NotImplementedException();
        }
        protected IVector MultT(IVector x)
        {
            throw new NotImplementedException();
        }
        protected IVector SolveLT(IVector x, bool UseDiagonal)
        {
            throw new NotImplementedException();
        }
        protected IVector SolveUT(IVector x, bool UseDiagonal)
        {
            throw new NotImplementedException();
        }
        protected IVector MultLT(IVector x, bool UseDiagonal)
        {
            throw new NotImplementedException();
        }
        protected IVector MultUT(IVector x, bool UseDiagonal)
        {
            throw new NotImplementedException();
        }
    }
    /*class CoordinateMatrix : IMatrix
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
    }*/
}
