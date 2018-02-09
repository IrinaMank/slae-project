using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;

namespace slae_project.Solver
{
    interface solver
    {
        Vector solve(IMatrix A, Vector b, double precision, int maxiter);
    }
}
