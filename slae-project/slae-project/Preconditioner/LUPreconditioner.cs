using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;
using slae_project.Vector;

namespace slae_project.Preconditioner
{
    public class LUPreconditioner : UseMatrixPreconditioner
    {
        public LUPreconditioner(IMatrix matr)
        {
            m = matr.Clone() as IMatrix;
            m.MakeLU();
        }
    }
}
