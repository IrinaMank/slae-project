using slae_project.Matrix;
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
        public IVector Solve(IMatrix A, IVector b, IVector Initial, double Precision, int Maxiter)
        {
            IVector x = new SimpleVector(b.Size);

            if (b.Norm == 0)
                return x;

            IVector r0 = b.Add(A.Mult(Initial), 1, -1); //r_0 = f - Ax_0
            IVector r = r0; // z_0 = r_0

            //заполнить вектора 0
            IVector v = r0, p = r0;
            v.SetConst();
            p.SetConst();

            IVector l, s, t;
            double z = 1.0, alpha = 1.0, omega = 1.0;
            double beta;
            double r_r = 0.0;

            for (int iter = 0; iter < Maxiter && r.ScalarMult(r) > Precision; iter++)
            {
                r_r = r0.ScalarMult(r); //zk = (r0, r(k-1))
                beta = (r_r * alpha) / (z * omega);//beta = (zk * alpha(k-1))/(z(k-1) * omega(k-1))
                l = p.Add(v, 1, -omega);//lk = p(k-1) - omega(k-1) * v(k-1)
                p = r.Add(l, 1, beta);//pk = r(k-1) + beta * lk
                v = A.Mult(p);//vk = A * pk
                alpha = r_r / (r0.ScalarMult(v));//alpha = zk/(r0, vk)
                s = r.Add(v, 1, -alpha);//sk = r(k-1) - alpha * vk
                t = A.Mult(s);//tk = A * sk
                omega = t.ScalarMult(s) / t.ScalarMult(t);//omega(k) = (tk, sk)/(tk, tk)
                x.Add(s, 1, omega, true);//xk = x(k-1) + omega(k) * sk
                x.Add(p, 1, alpha, true);//xk = x(k-1) + alpha(k) * pk
                r = s.Add(t, 1, -omega);//rk = sk - omega(k) * tk
            }
            return x;
        }
    }
}
