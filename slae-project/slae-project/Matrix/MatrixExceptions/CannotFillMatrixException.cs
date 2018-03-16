using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Matrix.MatrixExceptions
{
    class CannotFillMatrixException : ApplicationException
    {
        private static string base_message = "Не удалось выполнить заполнение матрицы из файлов. ";
        public CannotFillMatrixException() : base(base_message) { }
        public CannotFillMatrixException(string message) : base(base_message + message) { }
    }
}
