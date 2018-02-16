using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;
using slae_project.Vector;


namespace slae_project.Solver
{
    class Solver : ISolver
    {
        IVector ISolver.solve(IMatrix A, IVector b, IVector initial, double precision, int maxiter, SharedResources.Method m)
        {
            switch (m)
            {
                case SharedResources.Method.CGM:
                    return CGM(A, b, initial, precision, maxiter);
            }
            
            return new SimpleVector();
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
        IVector CGM(IMatrix A, IVector b, IVector initial, double precision, int maxiter)
        {
            IVector r = new SimpleVector();
            IVector z = new SimpleVector();
            IVector x = new SimpleVector(b.Size);
            double alpha,beta;
        
            r = b.Add(A.Mult(initial),1,-1);
            double r_r = r.ScalarMult(r);
            z = r;

            for (int iter = 0; iter< maxiter && r.Norm / b.Norm > precision;iter++)
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
