using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Matrix
{
    public static class MatrixConstants
    {
        /// <summary>
        /// СЛАУ несовместна. Решений нет.
        /// </summary>
        public const int SLAE_INCOMPATIBLE = -1;

        /// <summary>
        /// Слае имеет ровно одно решение
        /// </summary>
        public const int SLAE_OK = 0;

        /// <summary>
        /// СЛАУ имеет более одного решения.
        /// </summary>
        public const int SLAE_MORE_ONE_SOLUTION = 1;
    }
}
