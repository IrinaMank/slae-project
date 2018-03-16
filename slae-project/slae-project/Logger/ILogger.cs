using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Logger
{
    public interface ILogger
    {
        void WriteIteration(int number, double residual);
    }
}
