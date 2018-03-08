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
        public IVector Solve(IPreconditioner A, IVector b, IVector Initial, double Precision, int Maxiter, ILogger Logger)
        {
            IVector x = new SimpleVector(b.Size);

            if (b.Norm == 0)
                return x;

            double alpha, beta = 1.0;

            IVector r = b.Add(A.Matrix.Mult(Initial), 1, -1); //r_0 = f - Ax_0
            IVector Ar, z = r; // z_0 = r_0
            IVector p = A.Matrix.Mult(z); // p_0 = Az_0
            double p_r = 0.0, p_p = 0.0;

            //for (int iter = 0; iter < Maxiter && r.ScalarMult(r) > Precision && beta > 0; iter++)
            for (int iter = 0; iter < Maxiter ; iter++)
            {
                p_r = p.ScalarMult(r); //(p_k-1,r_k-1)
                p_p = p.ScalarMult(p); //(p_k-1,p_k-1)
                alpha = p_r / p_p;
                x.Add(z, 1, alpha, true); // x_k = x_k-1 = alfa_k*z_k-1
                r.Add(p, 1, -alpha, true); // r_k = r_k-1 - alfa_k*p_k-1
                Ar = A.Matrix.Mult(r); // Ar_k
                beta = -(p.ScalarMult(Ar) / p_p);
                z = r.Add(z, 1, beta); //z_k = r_k + beta_k*z_k-1
                p = Ar.Add(p, 1, beta); // p_k = Ar_k + beta_k*p_k-1
            }
            return x;
        }
    }
}
