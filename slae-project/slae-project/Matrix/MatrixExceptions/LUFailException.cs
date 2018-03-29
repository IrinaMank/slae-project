using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace slae_project.Matrix.MatrixExceptions
{
    public class LUFailException : ApplicationException
    {
        private static string base_message = "Использование  выбранного предобуславливания для данного СЛАУ невозможно. ";
        public LUFailException() : base(base_message) { }
        public LUFailException(string message) : base(base_message + message) { }
    }
}
