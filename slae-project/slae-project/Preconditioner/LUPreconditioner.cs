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
        public IMatrix matrix { get; }

        public LUPreconditioner(IMatrix matr)
        {
            matrix = (IMatrix)matr.Clone();
            matrix.MakeLU();
        }
        public IVector MultL(IVector v) => matrix.MultL(v);
        public IVector MultU(IVector v) => matrix.MultU(v);
        public IVector SolveL(IVector v) => matrix.SolveL(v);
        public IVector SolveU(IVector v) => matrix.SolveU(v);
    }
}
