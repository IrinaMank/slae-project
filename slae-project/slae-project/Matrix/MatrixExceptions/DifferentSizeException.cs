using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace slae_project.Matrix.MatrixExceptions
{
    [Serializable]
    public class DifferentSizeException : ApplicationException
    {

        public DifferentSizeException() { }

        public DifferentSizeException(string message) : base(message) { }

        public DifferentSizeException(string message, Exception inner) : base(message, inner) { }

        protected DifferentSizeException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
