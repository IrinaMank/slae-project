using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Matrix
{
    class Vector
    {
        public int size;
        public double[] elements;

        //Доступ к элементам вектора по индексу
        public double this[int key]
        {
            get
            {
                return elements[key];
            }
            set
            {
                elements[key] = value;
            }
        }
        public void SetZero()
        {
            for (int i = 0; i < size; i++)
                elements[i] = 0;
        }

        // Конструктор по умолчанию
        // Создает вектор размера [1]
        public Vector()
        {
            size = 1;
            elements = new double[size];
            SetZero();
        }

        // Создает вектор размера [n]
        public Vector(int n)
        {
            if (n > 0)
                size = n;
            else
                size = 1;
            elements = new double[size];
            SetZero();
        }

        // Инициализирует вектор массивом a
        public Vector(double[] a)
        {
            Set(a);
        }
        // Инициализирует вектор вектором
        // (Создается копия)
        public Vector(Vector a)
        {
            if (a != null)
            {
                size = a.size;
                a.elements.CopyTo(elements, 0);
            }
            else
            {
                size = 1;
                elements = new double[size];
            }
            SetZero();
        }

        // Создает вектор размера [n]
        public bool Resize(int to_n)
        {
            double[] new_vec;
            if (to_n > 0)
            {
                new_vec = new double[to_n];
                int min_size = Math.Min(to_n, size);
                for (int i = 0; i < min_size; i++)
                    new_vec[i] = this.elements[i];
                // В с# работает сборщик мусора, так что об утечках не стоит волноваться
                this.elements = new_vec;
                return true;
            }
            return false;
        }

        private void Set(double[] a)
        {
            if (a != null)
            {
                size = a.Length;
                // Потенциальное место ошибки
                // Не помню, выделяется ли память при копировании автоматически
                a.CopyTo(elements, 0);
            }
            else
            {
                size = 1;
                elements = new double[size];
            }
        }
        
        
        // Операция простого сложения векторов с коэффициентами
        // result = coef1 * this + coef2 * b
        // Удобно тем, что отсутвуют ненужные промежуточные результаты
        // Если _override == false, то результат НЕ записывается в вызывающую структуру, а возвращается как результат
        // Если _override == true, то результат записывается в вызывающую структуру, И возвращается как результат (return this)
        Vector Add(Vector b, double coef1 = 1.0, double coef2 = 1.0, bool _override = false)
        {
            double[] result;
            if (this.size == b.size)
            {
                result = new double[size];
                for (int i = 0; i < size; i++)
                    result[i] = coef1 * this.elements[i] + coef2 * b.elements[i];

                if (_override)
                {
                    this.Set(result);
                    return this;
                }
                else
                    return new Vector(result);

            }
            return null;
        }

        // Скалярное произведение вектора на b
        double ScalarMult(Vector b)
        {
            if (b.size == size)
            {
                double result = 0;
                for (int i = 0; i < size; i++)
                    result += this.elements[i] * b.elements[i];
                return result;
            }
            else
                return -1;
        }
        
        // Норма разности векторов
        // ||a - b||
        double Norm(Vector b)
        {
            if (b.size == size)
            {
                double result = 0;
                double temp;
                for (int i = 0; i < size; i++)
                {
                    temp = this.elements[i] - b.elements[i];
                    result += temp*temp;
                }
                return Math.Sqrt(result);
            }
            else
                return -1;
        }

        // Норма вектора
        // ||a||
        double Norm()
        {
            double result = 0;
            for (int i = 0; i < size; i++)
                result += this.elements[i] * this.elements[i];
            return Math.Sqrt(result);
        }

        // Оператор умножения матрицы на вектор
        public static Vector operator *(IMatrix A, Vector b)
        {
            return A.Mult(b);
        }

        // Оператор умножения обратной матрицы на вектор
        public static Vector operator /(IMatrix A, Vector b)
        {
            return A.Mult(b,true);
        }
    }
}
