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
        IMatrix Matrix { get; }

        IVector SMult(IVector v);
        IVector SSolve(IVector v);
        IVector QMult(IVector v);
        IVector QSolve(IVector v);
    }
}
