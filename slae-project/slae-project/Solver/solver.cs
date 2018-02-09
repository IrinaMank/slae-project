using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;


namespace slae_project.Solver
{
    class Solver : ISolver
    {
        Vector ISolver.solve(IMatrix A, Vector b, Vector initial, double precision, int maxiter, SharedResources.Method m)
        {
            switch (m)
            {
                case SharedResources.Method.CGM:
                    return CGM(A, b, initial, precision, maxiter);
            }
            
            return new Vector();
        }

        /// <summary>
        /// Решение СЛАУ методом сопряженных градиентов
        /// </summary>
        /// <param name="A">Матрица СЛАУ</param>
        /// <param name="b">Ветор правой части</param>
        /// <param name="initial">Ветор начального приближения</param>
        /// <param name="precision">Точность</param>
        /// <param name="maxiter">Максимальное число итераций</param>
        /// <returns>Вектор x - решение СЛАУ Ax=b с заданной точностью</returns>
        Vector CGM(IMatrix A, Vector b, Vector initial, double precision, int maxiter)
        {
            Vector r = new Vector();
            Vector z = new Vector();
            Vector x = new Vector(b.size);
            double alpha,beta;
        
            r = b.Add(A.Mult(initial));
            double r_r = r.ScalarMult(r);
            z = r;

            for (int iter = 0; iter< maxiter && r.Norm() / b.Norm() > precision;iter++)
            {
                r_r = r.ScalarMult(r);
                alpha = r_r / (A.Mult(z).ScalarMult(z));
                x.Add(z, 1, alpha, true);
                r.Add(A.Mult(z), 1, -alpha, true);

                beta = r_r;
                r_r = r.ScalarMult(r);
                beta = r_r / beta;

                z = r.Add(z, 1, beta);

            }
            return x;
        }
    }
}
