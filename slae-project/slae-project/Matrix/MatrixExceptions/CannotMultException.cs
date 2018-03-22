using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace slae_project.Matrix.MatrixExceptions
{
    class CannotMultException : ApplicationException
    {
        private static string base_message = "Не удалось выполнить умножение. ";
        public CannotMultException() : base(base_message) { }
        public CannotMultException(string message) : base(base_message + message) { }
    }
}
