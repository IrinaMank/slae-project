using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using slae_project.Preconditioner;
using slae_project.Vector;

namespace slae_project.Logger
{
    public class ConsoleLogger : ILogger
    {
        public ConsoleLogger()
        {
            AllocConsole();
        }
        public void WriteIteration(int number, double residual)
        {
            String msg = String.Format("Iteration: {0} \t residual: {1}", number, residual);
            System.Console.WriteLine(msg);
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();

        public void WriteSolution(IVector x, int Maxiter)
        {
            for (int i = 0; i<x.Size;i++)
            System.Console.Write("{1} ",x[i]);
            System.Console.WriteLine();
        }

        public void setMaxIter(int i)
        {
            throw new NotImplementedException();
        }

        public void WriteNameSolution(string nameSolver, string namePred)
        {
            throw new NotImplementedException();
        }

        public void WriteTime(string start, string end)
        {
            throw new NotImplementedException();
        }
    }
}
