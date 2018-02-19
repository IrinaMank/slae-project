using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace slae_project.Matrix.MatrixExceptions
{
    [Serializable]
    public class LUFailException : ApplicationException
    {
        private static string base_message = "Не удалось выполнить LU - разложение. ";
        public LUFailException() : base(base_message) { }

        public LUFailException(string message) : base(base_message + message) { }

        public LUFailException(string message, Exception inner) : base(base_message + message, inner) { }

        protected LUFailException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
