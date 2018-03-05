using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;
using slae_project.Vector;

namespace slae_project.Preconditioner
{
    public class NoPreconditioner : IPreconditioner
    {
        IMatrix matrix;
        public  NoPreconditioner(IMatrix matr)
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

        IVector IPreconditioner.QMult(IVector v)
        {
            return v;
        }

        IVector IPreconditioner.QSolve(IVector v)
        {
            return v;
        }

        IVector IPreconditioner.SMult(IVector v)
        {
            return v;
        }

        IVector IPreconditioner.SSolve(IVector v)
        {
            return v;
        }
    }
}
