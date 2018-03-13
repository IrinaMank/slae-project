using slae_project.Matrix;
using slae_project.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Preconditioner
{
    public interface IPreconditioner

    {
        IVector MultL(IVector v);
        IVector SolveL(IVector v);
        IVector MultU(IVector v);
        IVector SolveU(IVector v);
    }
    public class UseMatrixPreconditioner : IPreconditioner
    {
        protected IMatrix m;
        IVector IPreconditioner.MultL(IVector v) => m.MultL(v);
        IVector IPreconditioner.MultU(IVector v) => m.MultU(v);
        IVector IPreconditioner.SolveL(IVector v) => m.SolveL(v);
        IVector IPreconditioner.SolveU(IVector v) => m.SolveU(v);
    }
}
