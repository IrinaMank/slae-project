using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace slae_project.Matrix.MatrixExceptions
{
    public class SlaeNotCompatipableException : ApplicationException
    {
        private static string base_message = "Не удалось решить СЛАУ. ";
        public SlaeNotCompatipableException() : base(base_message) { }
        public SlaeNotCompatipableException(string message) : base(base_message + message) { }
    }
}
