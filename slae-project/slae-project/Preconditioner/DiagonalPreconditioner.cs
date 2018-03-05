using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;
using slae_project.Vector;

namespace slae_project.Preconditioner
{
    public class DiagonalPreconditioner : IPreconditioner
    {
        IMatrix matrix;
        public DiagonalPreconditioner(IMatrix matr)
        {
            matrix = matr;
        }

        public IMatrix Matrix
        {
            get
            {
                return matrix;
            }
        }


        public IVector QMult(IVector v)
        {
            throw new NotImplementedException();
        }

        public IVector QSolve(IVector v)
        {
            throw new NotImplementedException();
        }

        public IVector SMult(IVector v)
        {
            throw new NotImplementedException();
        }

        public IVector SSolve(IVector v)
        {
            throw new NotImplementedException();
        }
    }
}
