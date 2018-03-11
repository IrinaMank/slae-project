using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;
using slae_project.Vector;

namespace slae_project.Preconditioner
{
    class LUPreconditioner : IPreconditioner
    {
        public IMatrix matrix;

        public LUPreconditioner(IMatrix matr)
        {
            matrix = matr;
            matrix.MakeLU();
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
            return matrix.MultL(v);
        }

        public IVector QSolve(IVector v)
        {
            return matrix.SolveL(v);
        }

        public IVector SMult(IVector v)
        {
            return matrix.MultU(v);
        }

        public IVector SSolve(IVector v)
        {
            return matrix.SolveU(v);
        }
    }
}
