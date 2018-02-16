using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;
using slae_project.Vector;


namespace slae_project.Solver
{

    interface ISolver
    {
        IVector solve(IMatrix A, IVector B, IVector Initial, double Precision, int Maxiter);
    }
}
