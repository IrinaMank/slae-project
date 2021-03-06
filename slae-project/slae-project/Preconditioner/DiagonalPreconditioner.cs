﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;
using slae_project.Vector;

namespace slae_project.Preconditioner
{
    public class DiagonalPreconditioner : IPreconditioner
    {
        IMatrix matrix;
        public DiagonalPreconditioner(IMatrix matr)
        {
            matrix = matr.Clone() as IMatrix;
            if (matrix.Diagonal.ContainZero())
                throw new slae_project.Matrix.MatrixExceptions.LUFailException("Для использования диагонального предобуславливания главная диагональ исходной матрицы не должна содержать нулевые элементы.");
        }
        public class TransposeIllusion : IPreconditioner
        {
            public IMatrix Matrix { get; set; }
            public IPreconditioner T => this;
            public IVector MultL(IVector x) => Matrix.T.MultD(x);
            public IVector MultU(IVector x) => Matrix.T.MultD(x);
            public IVector SolveL(IVector x) => Matrix.T.SolveD(x);
            public IVector SolveU(IVector x) => Matrix.T.SolveD(x);
            string IPreconditioner.getName() => T.getName();

        }
        public IPreconditioner T => new TransposeIllusion { Matrix = matrix };
        IVector IPreconditioner.MultL(IVector v) => matrix.MultD(v);
        IVector IPreconditioner.MultU(IVector v) => matrix.MultD(v);
        IVector IPreconditioner.SolveL(IVector v) => matrix.SolveD(v);
        IVector IPreconditioner.SolveU(IVector v) => matrix.SolveD(v);
        string IPreconditioner.getName() => "Диагональное предобуславливание";
    }
}
