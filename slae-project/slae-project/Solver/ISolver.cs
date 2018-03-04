using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;
using slae_project.Vector;
using slae_project.Preconditioner;
using slae_project.ILogger;

namespace slae_project.Solver
{

    public interface ISolver
    {
        IVector Solve(IPreconditioner A, IVector B, IVector Initial, double Precision, int Maxiter, Logger lgoger);
        
    }
}
