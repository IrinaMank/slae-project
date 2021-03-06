﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using slae_project.Matrix;
using slae_project.Vector.VectorExceptions;
using slae_project.Vector;
using slae_project.Solver;
using slae_project.Preconditioner;
using slae_project.Logger;
using System.IO;
using System.Collections.Generic;
namespace UnitTestProject
{
    [TestClass]
    public class LOSTest
    {
        [TestMethod]
        public void RevertDiagonalLOS()
        {
            using (FileLogger logger = new FileLogger())
            {
                (int, int)[] coord = new(int, int)[4];
                double[] valMatrix = new double[4] { 1, 1, 1, 1 };
                double[] valB = new double[] { 4, 3, 2, 1 };
                double[] valX = new double[] { 1, 2, 3, 4 };

                coord[0] = (0, 3);
                coord[1] = (1, 2);
                coord[2] = (2, 1);
                coord[3] = (3, 0);


                IMatrix mar = new CoordinateMatrix(coord, valMatrix);
                IPreconditioner prec = new NoPreconditioner();
                IVector b = new SimpleVector(valB);
                //IVector x0 = new SimpleVector(5);
                IVector x0 = new SimpleVector(4);
                IVector rigth_X = new SimpleVector(valX);
                ILogger logg2 = logger.returnThis();
                ISolver s = new LOSSolver();
                IVector x = s.Solve(prec, mar, b, x0, 1e-8, 10000, logg2);

                Assert.IsTrue(x.CompareWith(rigth_X, 1e-8));
            }
        }

        [TestMethod]
        public void x0TestLOS()
        {
            //МСГ - зависит от нач приближения.
            using (FileLogger logger = new FileLogger())
            {
                (int, int)[] coord = new(int, int)[4];
                double[] valMatrix = new double[4] { 1, 1, 1, 1 };
                double[] valB = new double[] { 1, 2, 3, 4 };
                double[] valX = new double[] { 1, 2, 3, 4 };

                coord[0] = (0, 0);
                coord[1] = (1, 1);
                coord[2] = (2, 2);
                coord[3] = (3, 3);


                IMatrix mar = new CoordinateMatrix(coord, valMatrix);
                IPreconditioner prec = new NoPreconditioner();
                IVector b = new SimpleVector(valB);
                //IVector x0 = new SimpleVector(5);
                double[] x00 = new double[] { 1, 1, 1, 1 };
                IVector x01 = new SimpleVector(x00);

                double[] x002 = new double[] { 0, 0, 0, 0 };
                IVector x02 = new SimpleVector(x002);

                IVector rigth_X = new SimpleVector(valX);

                ISolver s = new LOSSolver();
                ILogger logg2 = logger.returnThis();
                IVector x1 = s.Solve(prec, mar, b, x01, 1e-8, 10000, logg2);
                ILogger logg = logger.returnThis();
                IVector x2 = s.Solve(prec, mar, b, x02, 1e-8, 10000, logg);
                Assert.IsTrue(x1.CompareWith(x2, 1e-8));
            }
        }

        [TestMethod]
        public void PrimTestLOS()
        {
            using (FileLogger logger = new FileLogger())
            {
                (int, int)[] coord = new(int, int)[4];
                double[] valMatrix = new double[4] { 1, 1, 1, 1 };
                double[] valB = new double[] { 1, 2, 3, 4 };
                double[] valX = new double[] { 1, 2, 3, 4 };

                coord[0] = (0, 0);
                coord[1] = (1, 1);
                coord[2] = (2, 2);
                coord[3] = (3, 3);


                IMatrix mar = new CoordinateMatrix(coord, valMatrix);
                IPreconditioner prec = new NoPreconditioner();
                IVector b = new SimpleVector(valB);
                //IVector x0 = new SimpleVector(5);
                IVector x0 = new SimpleVector(4);
                IVector rigth_X = new SimpleVector(valX);

                ISolver s = new LOSSolver();
                ILogger logg2 = logger.returnThis();
                IVector x = s.Solve(prec, mar, b, x0, 1e-2, 10000, logg2);

                Assert.IsTrue(x.CompareWith(rigth_X, 1e-2));
            }
        }

        [TestMethod]
        public void CalculateNumbers()
        {
            using (FileLogger logger = new FileLogger())
            {
                double[,] val = new double[4, 4] { { 1, 2, 3, 4 }, { 2, 2, 3, 4 }, { 3, 3, 3, 4 }, { 4, 4, 4, 4 } };
                IMatrix mar = new DenseMatrix(val);
                double[] valB = new double[] { 30, 31, 34, 40 };
                double[] valX = new double[] { 1, 2, 3, 4 };
                IPreconditioner prec = new NoPreconditioner();
                IVector b = new SimpleVector(valB);
                IVector x0 = new SimpleVector(4);
                IVector rigth_X = new SimpleVector(valX);

                ISolver s = new LOSSolver();
                ILogger logg2 = logger.returnThis();
                IVector x1 = s.Solve(prec, mar, b, x0, 1e-10, 10000, logg2);
                ILogger logg = logger.returnThis();
                IVector x2 = s.Solve(new LUPreconditioner(mar), mar, b, x0, 1e-10, 10000, logg);
                ILogger logg3 = logger.returnThis();
                IVector x3 = s.Solve(new DiagonalPreconditioner(mar), mar, b, x0, 1e-10, 10000, logg3);
                Assert.IsTrue(x1.CompareWith(rigth_X, 1e-8), "NoPrec");
                Assert.IsTrue(x2.CompareWith(rigth_X, 1e-8), "LUPrec");
                Assert.IsTrue(x3.CompareWith(rigth_X, 1e-8), "DiPrec");

            }
        }

        [TestMethod]
        public void UMatrix()
        {
            using (FileLogger logger = new FileLogger())
            {
                double[,] val = new double[4, 4] { { 1, 1, 1, 1 }, { 0, 1, 1, 1 }, { 0, 0, 1, 1 }, { 0, 0, 0, 1 } };
                IMatrix mar = new DenseMatrix(val);
                double[] valB = new double[] { 4, 3, 2, 1 };
                double[] valX = new double[] { 1, 1, 1, 1 };
                IPreconditioner prec = new NoPreconditioner();
                IVector b = new SimpleVector(valB);
                IVector x0 = new SimpleVector(4);
                IVector rigth_X = new SimpleVector(valX);

                ISolver s = new LOSSolver();
                ILogger logg2 = logger.returnThis();
                IVector x1 = s.Solve(prec, mar, b, x0, 1e-10, 10000, logg2);
                ILogger logg = logger.returnThis();
                IVector x2 = s.Solve(new LUPreconditioner(mar), mar, b, x0, 1e-10, 10000, logg);
                ILogger logg3 = logger.returnThis();
                IVector x3 = s.Solve(new DiagonalPreconditioner(mar), mar, b, x0, 1e-10, 10000, logg3);
                Assert.IsTrue(x1.CompareWith(rigth_X, 1e-8), "NoPrec");
                Assert.IsTrue(x2.CompareWith(rigth_X, 1e-8), "LUPrec");
                Assert.IsTrue(x3.CompareWith(rigth_X, 1e-8), "DiPrec");

            }
        }
    }
}
