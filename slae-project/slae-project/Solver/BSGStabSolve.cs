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
    public class BSGStabSolve : ISolver
    {
        /// <summary>
        /// Решение СЛАУ стабилизированным методом бисопряжённых градиентов
        /// </summary>
        /// <param name="A">Матрица СЛАУ</param>
        /// <param name="b">Ветор правой части</param>
        /// <param name="Initial">Ветор начального приближения</param>
        /// <param name="Precision">Точность</param>
        /// <param name="Maxiter">Максимальное число итераций</param>
        /// <returns>Вектор x - решение СЛАУ Ax=b с заданной точностью</returns>

        public IVector Solve(IPreconditioner Preconditioner, IMatrix A, IVector b, IVector Initial, double Precision, int Maxiter, ILogger Logger)
        {
            IVector x = (IVector)Initial.Clone();

            if (b.Norm == 0)
                return x;

            IVector r0 = b.Add(A.Mult(Initial), 1, -1); //r_0 = f - Ax_0
            r0 = Preconditioner.MultL(r0);//r_0 = L(-1)(f - Ax_0)
            IVector z = Preconditioner.MultU(r0);//z_0 = U(-1)r_0

            IVector r = (IVector)r0.Clone(); // r = r_0

            IVector p = new SimpleVector(b.Size);
            IVector LAUz = new SimpleVector(b.Size);
            IVector LAUp = new SimpleVector(b.Size);

            double alpha, beta, gamma;
            double r_r, r_r_1;

            double normR = r.Norm / b.Norm;

            r_r = r0.ScalarMult(r);//(r(k-1),r0)

            for (int iter = 0; iter < Maxiter && normR > Precision; iter++)
            {
                LAUz = Preconditioner.SolveU(Preconditioner.MultU(z));//U(-1)z(k - 1)
                LAUz = A.Mult(LAUz);//AU(-1)z(k-1)
                LAUz = Preconditioner.MultL(LAUz);//L(-1)AU(-1)z(k-1)

                alpha = r_r / r0.ScalarMult(LAUz);//alpha = (r(k-1),r0)/(r0,L(-1)AU(-1)z(k-1))

                p = r.Add(LAUz, 1, -alpha);//pk = r(k-1) - alpha * L(-1)AU(-1)z(k-1)

                LAUp = Preconditioner.MultU(p);//U(-1)p(k)
                LAUp = A.Mult(LAUp);//AU(-1)p(k)
                LAUp = Preconditioner.MultL(LAUp);//L(-1)AU(-1)p(k)

                gamma = p.ScalarMult(LAUp) / LAUp.ScalarMult(LAUp);//gamma = (p(k),L(-1)AU(-1)p(k))/(L(-1)AU(-1)p(k),L(-1)AU(-1)p(k))

                x.Add(z, 1, alpha, true);//xk = x(k-1) + alpha(k) * z(k-1)
                x.Add(p, 1, gamma, true);//xk = x(k-1) + gamma(k) * p(k)

                r_r_1 = r0.ScalarMult(r);//(r(k-1),r0)

                r = p.Add(LAUp, 1, -gamma);//rk = p(k) - gamma(k) * L(-1)AU(-1)p(k)

                r_r = r0.ScalarMult(r);//(r(k), r0)

                beta = (r_r * alpha) / (r_r_1 * gamma);//beta = ((r(k),r0) * alpha(k))/((r(k-1),r0) * omega(k-1))

                z = r.Add(z, 1, beta);//z(k) = r(k) + beta(k) * z(k-1)
                z.Add(LAUz, 1, -beta * gamma, true);//z(k) = z(k) - beta(k) * gamma(k) * L(-1)AU(-1)z(k-1)

                normR = r.Norm / b.Norm;

                Logger.WriteIteration(iter, normR);
            }
            x = Preconditioner.MultU(x);//x = U(-1)x
            return x;
        }
    }
}
