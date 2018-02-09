﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;


namespace slae_project.Solver
{

    interface ISolver
    {       
        
        Vector solve(IMatrix A, Vector b, Vector initial, double precision, int maxiter, SharedResources.Method m);
    }
}
