using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace slae_project.Matrix.MatrixExceptions
{
    [Serializable]
    public class CannotSolveSLAEExcpetion : ApplicationException
    {

        private static string base_message = "Не удалось решить СЛАУ. ";
        public CannotSolveSLAEExcpetion() : base(base_message) { }

        public CannotSolveSLAEExcpetion(string message) : base(base_message + message) { }

        public CannotSolveSLAEExcpetion(string message, Exception inner) : base(base_message + message, inner) { }

        protected CannotSolveSLAEExcpetion(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
