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
            matrix = matr.Clone() as IMatrix;
        }

        IVector IPreconditioner.MultL(IVector v) => matrix.SolveD(v);
        IVector IPreconditioner.MultU(IVector v) => matrix.SolveD(v);
        IVector IPreconditioner.SolveL(IVector v) => matrix.SolveD(v);
        IVector IPreconditioner.SolveU(IVector v) => matrix.SolveD(v);
    }
}
