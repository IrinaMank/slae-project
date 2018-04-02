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
        public class TransposeIllusion : IPreconditioner
        {
            public IPreconditioner T => this;

            public IVector MultL(IVector x) => x.Clone() as IVector;
            public IVector SolveL(IVector x) => x.Clone() as IVector;
            public IVector MultU(IVector x) => x.Clone() as IVector;
            public IVector SolveU(IVector x) => x.Clone() as IVector;

            public string getName() => T.getName();
        }
        public IPreconditioner T => new TransposeIllusion {  };

        public string getName() => "Без предобуславливания";
        IVector IPreconditioner.MultL(IVector v) => v.Clone() as IVector;
        IVector IPreconditioner.MultU(IVector v) => v.Clone() as IVector;
        IVector IPreconditioner.SolveL(IVector v) => v.Clone() as IVector;
        IVector IPreconditioner.SolveU(IVector v) => v.Clone() as IVector;
        }
    }

