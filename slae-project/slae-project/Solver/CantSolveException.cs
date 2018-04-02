using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Solver
{
    class CantSolveException : ApplicationException
    {
        private static string base_message = "метод не может справиться с этой СЛАУ. ";
        public CantSolveException() : base(base_message) { }
        public CantSolveException(string message) : base(base_message + message) { }
    }
}
