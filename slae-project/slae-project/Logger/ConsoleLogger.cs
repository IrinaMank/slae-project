﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
    }
}