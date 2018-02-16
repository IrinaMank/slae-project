using slae_project.Matrix;
using slae_project.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Solver
{
    public class MSGSolver : ISolver
    {
        /// <summary>
        /// Решение СЛАУ методом сопряженных градиентов
        /// </summary>
        /// <param name="A">Матрица СЛАУ</param>
        /// <param name="b">Ветор правой части</param>
        /// <param name="Initial">Ветор начального приближения</param>
        /// <param name="Precision">Точность</param>
        /// <param name="Maxiter">Максимальное число итераций</param>
        /// <returns>Вектор x - решение СЛАУ Ax=b с заданной точностью</returns>
        public IVector Solve(IMatrix A, IVector b, IVector Initial, double Precision, int Maxiter)
        {
            IVector x = new SimpleVector(b.Size);

            if (b.Norm == 0)
                return x;

            double alpha, beta = 1.0;

            IVector r = b.Add(A.Mult(Initial), 1, -1);
            double r_r = r.ScalarMult(r);
            IVector Az, z = r;

            for (int iter = 0; iter < Maxiter && r.Norm / b.Norm > Precision && beta > 0; iter++)
            {
                Az = A.Mult(z);
                r_r = r.ScalarMult(r);
                alpha = r_r / (Az.ScalarMult(z));
                x.Add(z, 1, alpha, true);
                r.Add(Az, 1, -alpha, true);

                beta = r_r;
                r_r = r.ScalarMult(r);
                beta = r_r / beta;

                z = r.Add(z, 1, beta);
            }
            return x;
        }
    }
}
