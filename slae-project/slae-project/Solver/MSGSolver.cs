﻿using slae_project.Logger;
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
        public IVector Solve(IPreconditioner A, IVector b, IVector Initial, double Precision, int Maxiter, ILogger Logger)
        {
            IVector x = Initial.Clone() as IVector;

            if (b.Norm == 0)
                return x;

            double scalAzZ, scalRR, alpha, beta = 1.0;

            IVector r = b.Add(A.Matrix.Mult(Initial), 1, -1);
            r = A.SSolve(A.SMult(r));
            IVector Az, Atz, z = A.Matrix.Transpose.Mult(r);
            z = A.QMult(z);

            r = z.Clone() as IVector;
            scalRR = r.ScalarMult(r);
            double normR = Math.Sqrt(scalRR) / b.Norm;

            for (int iter = 0; iter < Maxiter && normR > Precision && beta > 0; iter++)
            {
                Az = A.QSolve(z);

                Atz = A.Matrix.Mult(Az);
                Atz = A.SSolve(A.SMult(Atz));
                Az = A.Matrix.Transpose.Mult(Atz);
                Az = A.QMult(Az);

                scalAzZ = Az.ScalarMult(z);

                if (scalAzZ == 0) throw new Exception("Division by 0");

                alpha = scalRR / scalAzZ;

                x.Add(z, 1, alpha, true);
                r.Add(Az, 1, -alpha, true);

                beta = scalRR;
                if (scalRR == 0) throw new Exception("Division by 0");
                scalRR = r.ScalarMult(r);
                beta = scalRR / beta;

                z = r.Add(z, 1, beta);
                normR = Math.Sqrt(scalRR) / b.Norm;

                Logger.WriteIteration(iter, normR);
            };
            return A.SSolve(x);
        }
    }
}
