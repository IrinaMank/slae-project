using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace slae_project.Matrix.MatrixExceptions
{
    [Serializable]
    class CannotMultException : ApplicationException
    {
        private static string base_message = "Не удалось выполнить умножение. ";
        public CannotMultException() : base(base_message) { }

        public CannotMultException(string message) : base(base_message + message) { }

        public CannotMultException(string message, Exception inner) : base(base_message + message, inner) { }

        protected CannotMultException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
