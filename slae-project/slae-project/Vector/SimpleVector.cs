using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Vector.VectorExceptions;

namespace slae_project.Vector
{
    public class SimpleVector : IVector
    {
        public int Size { get; }

        public double Norm {
            get
            {
                double result = 0;
                for (int i = 0; i < Size; i++)
                    result += this[i] * this[i];
                return Math.Sqrt(result);
            }
        }

    public double[] elements;

        //Доступ к элементам вектора по индексу
        public double this[int key]
        {
            get
            {
                try
                {
                    return elements[key];
                }
                catch
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                elements[key] = value;
            }
        }
        
       public SimpleVector()
        {

        }
        /// <summary>
        /// Нулевой вектор размерности m
        /// </summary>
        /// <param name="m">Размерность вектора</param>
        public SimpleVector(int m)
        {
            if (m > 0)
                Size = m;
            else
                throw new WrongSizeException();
            elements = new double[Size];
            SetConst();
        }

        public SimpleVector(double[] b)
        {
            Size = b.Length;
            elements = new double[Size];
            b.CopyTo(elements,0);
        }

        public IVector Add(IVector b, double coef1 = 1.0, double coef2 = 1.0, bool _override = false)
        {
            if (this.Size == b.Size)
            {
                double[] result = new double[Size];
                for (int i = 0; i < Size; i++)
                    result[i] = coef1 * this[i] + coef2 * b[i];

                if (_override)
                {
                    this.elements = result;
                    return this;
                }
                else
                    return new SimpleVector(result);

            }
            MessageBox.Show("Попытка сложить вектора разных размерностей. Метод 'Add' вернул null. В следующий раз будь аккуратнее :3",
                "Исключение",MessageBoxButtons.OK,MessageBoxIcon.Error);

            return null;
        }
        
        public double ScalarMult(IVector b)
        {
            if (b.Size == Size)
            {
                double result = 0;
                for (int i = 0; i < Size; i++)
                    result += this[i] * b[i];
                return result;
            }
            MessageBox.Show("Попытка найти скалярное произведение векторов разных размерностей. Метод 'ScalarMult' вернул -1. В следующий раз будь аккуратнее :3",
                "Исключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return -1;
        }

        public void SetConst(double v = 0)
        {
            for (int i = 0; i < Size; i++)
                elements[i] = v;
        }
        public bool CompareWith(IVector a, double prec = 1e-5)
        {
            //ПМ-43 восхитительны
            if (this.Size == a.Size)
            {
                for (int i = 0; i < Size; i++)
                {
                    if (this[i] + prec > a[i] &&
                        this[i] - prec < a[i])
                        continue;
                    return false;
                }

                return true;
            }
            throw new WrongSizeException();
        }
        public IEnumerator<(double value, int index)> GetEnumerator()
        {
            IEnumerable<(double value, int index)> F()
            {
                for(int i=0;i<Size;i++)
                    yield return (elements[i], i);
            }
            return F().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object Clone()
        {
            SimpleVector clon = new SimpleVector(this.elements);
            return clon;
        }

    }
}
