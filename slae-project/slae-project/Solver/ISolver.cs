using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;


namespace slae_project.Solver
{

    interface ISolver
    {

        IVector solve(IMatrix A, IVector b, IVector initial, double precision, int maxiter, SharedResources.Method m);
    }
}
