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
           return matrix.SolveD(v);
        }

        public IVector QSolve(IVector v)
        {
            return matrix.SolveD(v);
        }

        public IVector SMult(IVector v)
        {
            return matrix.SolveD(v);
        }

        public IVector SSolve(IVector v)
        {
            return matrix.SolveD(v);
        }
    }
}
