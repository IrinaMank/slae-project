using slae_project.Logger;
using slae_project.Matrix;
using slae_project.Preconditioner;
using slae_project.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Solver
{
    public class LOSSolver : ISolver
    {
        /// <summary>
        /// Решение СЛАУ методом локально-оптимальной схемы
        /// </summary>
        /// <param name="A">Матрица СЛАУ</param>
        /// <param name="b">Ветор правой части</param>
        /// <param name="Initial">Ветор начального приближения</param>
        /// <param name="Precision">Точность</param>
        /// <param name="Maxiter">Максимальное число итераций</param>
        /// <returns>Вектор x - решение СЛАУ Ax=b с заданной точностью</returns>
        public IVector Solve(IPreconditioner A, IMatrix AA, IVector b, IVector Initial, double Precision, int Maxiter, ILogger Logger)
        {
            IVector x = new SimpleVector(b.Size);

            if (b.Norm == 0)
                return x;

            double alpha = 0.0, beta = 0.0;

            IVector r = b.Add(AA.Mult(Initial), 1, -1); //r_0 = f - Ax_0
            r = A.SolveU(r); // r_0 = L^-1 * (f - Ax_0)

            IVector Ar, z = A.SolveL(r); // z_0 = U^-1 * r_0
            IVector p = A.SolveU(AA.Mult(z)); // p_0 = L^-1 * Az_0

            double p_r = 0.0, p_p = 0.0;

            double scalRR = r.ScalarMult(r);
            double normR = Math.Sqrt(scalRR) / b.Norm;

            for (int iter = 0; iter < Maxiter && normR > Precision; iter++)
            //for (int iter = 0; iter < Maxiter && ; iter++)
            {
                p_r = p.ScalarMult(r); //(p_k-1,r_k-1)
                p_p = p.ScalarMult(p); //(p_k-1,p_k-1)
                alpha = p_r / p_p;

                x.Add(z, 1, alpha, true); // x_k = x_k-1 + alfa_k*z_k-1

                r.Add(p, 1, -alpha, true); // r_k = r_k-1 - alfa_k*p_k-1

                // Ar = A.QSolve(AA.Mult(A.SSolve(r))); //Ar_k = L^-1 * A * U^-1 * r_k
                Ar = A.SolveL(r);
                Ar = AA.Mult(Ar);
                Ar = A.SolveU(Ar);

                beta = -(p.ScalarMult(Ar) / p_p);

                z = A.SolveL(r).Add(z, 1, beta); //z_k = U^-1 * r_k + beta_k*z_k-1
                p = Ar.Add(p, 1, beta); // p_k = L^-1 * A * U^-1 * r_k + beta_k*p_k-1
                if (scalRR == 0) throw new Exception("Division by 0");
                scalRR = r.ScalarMult(r);
                normR = Math.Sqrt(scalRR) / b.Norm;
            }
            return x;
        }

    }
}
