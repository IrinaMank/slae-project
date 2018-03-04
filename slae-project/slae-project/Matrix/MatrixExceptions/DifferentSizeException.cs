using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace slae_project.Matrix.MatrixExceptions
{
    public class DifferentSizeException : ApplicationException
    {
        private static string base_message = "Используемые объекты имеют разную размерность. ";
        public DifferentSizeException() : base(base_message) { }
        public DifferentSizeException(string message) : base(base_message + message) { }
    }
}
