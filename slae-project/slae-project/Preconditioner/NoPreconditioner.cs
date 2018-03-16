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
        IVector IPreconditioner.MultL(IVector v) => v.Clone() as IVector;
        IVector IPreconditioner.MultU(IVector v) => v.Clone() as IVector;
        IVector IPreconditioner.SolveL(IVector v) => v.Clone() as IVector;
        IVector IPreconditioner.SolveU(IVector v) => v.Clone() as IVector;
    }
}
