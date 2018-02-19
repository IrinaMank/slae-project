using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace slae_project.Vector.VectorExceptions
{
    [Serializable]
    public class WrongSizeException : ApplicationException
    {
        
        public WrongSizeException() { }

        public WrongSizeException(string message) : base(message) { }

        public WrongSizeException(string message, Exception inner) : base(message, inner) { }

        protected WrongSizeException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
