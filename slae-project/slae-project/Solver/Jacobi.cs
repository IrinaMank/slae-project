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
    public class Jacobi : ISolver
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
        public IVector Solve(IPreconditioner Preconditioner, IMatrix A, IVector b, IVector Initial, double Precision, int Maxiter, ILogger Logger)
        {
            Logger.WriteNameSolution("Jacobi", Preconditioner.getName());
            string start = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss:fff");

            Logger.setMaxIter(Maxiter);
            IVector x = (IVector)Initial.Clone();//начальное приблежение

            IVector r = b.Add(A.Mult(x), 1, -1);//r = b - Ax_0
            double residual = r.Norm / b.Norm;// ||b-Ax_0|| / ||b||

            for (int i = 0; i < Maxiter && residual > Precision; i++)
            {
                x = A.SolveD(b.Add(A.Mult(x, false), 1, -1));//x_k+1=D_-1 (b-Rx_k)

                r = b.Add(A.Mult(x), 1, -1);//r = b - Ax
                residual = r.Norm / b.Norm;// ||b-Ax|| / ||b||
                Factory.Residual.Add(residual);
                Logger.WriteIteration(i, residual);

                if (double.IsNaN(r.Norm) || double.IsInfinity(r.Norm))
                    throw new CantSolveException();
            }
            Logger.WriteSolution(x, Maxiter, b.Add(A.Mult(x), -1, 1).Norm);
            Logger.WriteTime(start, DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss:fff"));
            return x;
        }

    }
}
