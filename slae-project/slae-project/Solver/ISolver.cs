using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;
using slae_project.Vector;
using slae_project.Preconditioner;
using slae_project.Logger;

namespace slae_project.Solver
{

    public interface ISolver
    {
        IVector Solve(IPreconditioner Preconditioner,IMatrix A, IVector B, IVector Initial, double Precision, int Maxiter, ILogger Logger );
        
    }
}
