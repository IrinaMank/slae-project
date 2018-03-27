using slae_project.Matrix;
using slae_project.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Preconditioner
{
    public interface IPreconditioner
    {
        /// <summary>
        /// Умножение вектора v на нижнюю треугольную матрицу
        /// </summary>
        /// <param name="v">Умножаемый вектор</param>
        /// <returns></returns>
        IVector MultL(IVector v);

        /// <summary>
        /// Умножение вектора v на верхнюю треугольную матрицу
        /// </summary>
        /// <param name="v">Умножаемый вектор</param>
        /// <returns></returns>
        IVector MultU(IVector v);

        /// <summary>
        /// Решить СЛАУ с нижней треугольной матрицей относительно вектора v
        /// </summary>
        /// <param name="v">Правая часть СЛАУ</param>
        /// <returns></returns>
        IVector SolveL(IVector v);

        /// <summary>
        /// Решить СЛАУ с верхней треугольной матрицей относительно вектора v
        /// </summary>
        /// <param name="v">Правая часть СЛАУ</param>
        /// <returns></returns>
        IVector SolveU(IVector v);
        IPreconditioner T { get; }
    }

    public class UseMatrixPreconditioner : IPreconditioner
    {
        protected IMatrix m;
        public class TransposeIllusion : IPreconditioner
        {
            public IMatrix Matrix { get; set; }
            public IPreconditioner T => this;
            public IVector MultL(IVector x) => Matrix.T.MultL(x);
            public IVector SolveL(IVector x) => Matrix.T.SolveL(x);
            public IVector MultU(IVector x) => Matrix.T.MultU(x);
            public IVector SolveU(IVector x) => Matrix.T.SolveU(x);
        }
        public IPreconditioner T => new TransposeIllusion { Matrix = m };
        IVector IPreconditioner.MultL(IVector v) => m.MultL(v);
        IVector IPreconditioner.MultU(IVector v) => m.MultU(v);
        IVector IPreconditioner.SolveL(IVector v) => m.SolveL(v);
        IVector IPreconditioner.SolveU(IVector v) => m.SolveU(v);

    }
}
