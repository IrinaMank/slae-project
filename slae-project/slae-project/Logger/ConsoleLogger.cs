using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void WriteIteration(int number, double residual)
        {
            String msg = String.Format("Iteration: {0} \t residual: {1}", number, residual);
            Console.WriteLine(msg);
        }
    }
}
