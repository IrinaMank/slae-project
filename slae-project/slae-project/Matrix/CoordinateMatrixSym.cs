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
using slae_project.Preconditioner;
using System.IO;

namespace slae_project.Matrix
{
    public class CoordinateMatrixSym : CoordinateMatrix
    {
        public CoordinateMatrixSym()
        {

        }
        public new double this[int i, int j]
        {

            get
            {
                if (i < this.Size && j < this.Size && i >= 0 && j >= 0)
                {
                    if (i>j)
                        return elements[(i, j)];
                    else
                        return elements[(j, i)];

                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (i < this.Size && j < this.Size && i >= 0 && j >= 0)
                {
                    if (value != 0)
                    {
                        if (i > j)
                            elements[(i, j)] = value;
                        else
                            elements[(j, i)] = value;

                    }
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }
        public new IVector Mult(IVector x, bool UseDiagonal = true)
        {
            if (this.Size != x.Size)
                throw new DifferentSizeException("Размерность матрицы не совпадает с размерностью вектора.");

            IVector result = new SimpleVector(Size);
            if (UseDiagonal)
            {
                foreach (var el in elements)
                {
                    result[el.Key.i] += el.Value * x[el.Key.j];
                    result[el.Key.j] += el.Value * x[el.Key.i];
                }
            }
            else
            {
                foreach (var el in elements)
                {
                    if (el.Key.i != el.Key.j)
                    {
                        result[el.Key.i] += el.Value * x[el.Key.j];
                        result[el.Key.j] += el.Value * x[el.Key.i];
                    }
                }
            }
            return result;
        }
        public new IVector MultL(IVector x, bool UseDiagonal = true)
        {
            throw new NotImplementedException();
        }
        public new IVector MultU(IVector x, bool UseDiagonal = true)
        {
            throw new NotImplementedException();
        }
        public new IVector MultUT(IVector x, bool UseDiagonal = true)
        {
            throw new NotImplementedException();
        }
        public new IVector MultLT(IVector x, bool UseDiagonal = true)
        {
            throw new NotImplementedException();
        }
        public new IVector MultT(IVector x, bool UseDiagonal = true)
        {
            throw new NotImplementedException();
        }
    }
}