using System;
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
    [TestCategory("Jacobi")]
    public class JacobiTest
    {
        [TestMethod]
        [TestCategory("Jacobi")]
        public void RevertDiagonalJacobi()
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

                ISolver s = new Jacobi();
                IVector x = s.Solve(prec, mar, b, x0, 1e-8, 10000, logger);

                Assert.IsTrue(x.CompareWith(rigth_X, 1e-8));
            }
        }

        [TestMethod]
        [TestCategory("Jacobi")]
        public void x0TestJacobi()
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

                ISolver s = new Jacobi();
                IVector x1 = s.Solve(prec, mar, b, x01, 1e-8, 10000, logger);
                IVector x2 = s.Solve(prec, mar, b, x02, 1e-8, 10000, logger);
                Assert.IsTrue(x1.CompareWith(x2, 1e-8));
            }
        }

        [TestMethod]
        [TestCategory("Jacobi")]
        public void PrimTestJacobi()
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

                ISolver s = new Jacobi();
                IVector x = s.Solve(prec, mar, b, x0, 1e-2, 10000, logger);

                Assert.IsTrue(x.CompareWith(rigth_X, 1e-2));
            }
        }

        [TestMethod]
        [TestCategory("Jacobi")]
        public void NegNumbers()
        {
            using (FileLogger logger = new FileLogger())
            {
                double[,] val = new double[4, 4] { { 1, 1, 1, 0 }, { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, -1 } };
                IMatrix mar = new DenseMatrix(val);
                double[] valB = new double[] { 1, 1, 1, 1 };
                double[] valX = new double[] { 1, 1, -1, -1 };
                IPreconditioner prec = new NoPreconditioner();
                IVector b = new SimpleVector(valB);
                IVector x0 = new SimpleVector(4);
                IVector rigth_X = new SimpleVector(valX);

                ISolver s = new Jacobi();
                IVector x1 = s.Solve(prec, mar, b, x0, 1e-10, 10000, logger);
                Assert.IsTrue(x1.CompareWith(rigth_X, 1e-8), "NoPrec");

            }
        }

        [TestMethod]
        [TestCategory("Jacobi")]
        public void NotNullx0()
        {
            using (FileLogger logger = new FileLogger())
            {
                double[,] val = new double[6, 6] { { 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 0, 0, 0 }, { 1, 1, 1, 1, 1, 0 }, { 1, 0, 1, 1, 1, 0 }, { 1, 0, 1, 1, 1, 1 }, { 1, 0, 0, 0, 1, 1 } };
                IMatrix mar = new DenseMatrix(val);
                double[] valB = new double[] { 1, 1, 1, 1, 1, 1 };
                double[] valX0 = new double[] { 1, 1, 1, 1, 1, 1 };
                double[] valX = new double[] { 0.74999999999999989, 0, 0.25, -0.24999999999999972, 0.25, 0 };
                IPreconditioner prec = new NoPreconditioner();
                IVector b = new SimpleVector(valB);
                IVector x0 = new SimpleVector(valX0);
                IVector rigth_X = new SimpleVector(valX);

                ISolver s = new Jacobi();
                IVector x1 = s.Solve(prec, mar, b, x0, 1e-10, 10000, logger);
                 Assert.IsTrue(x1.CompareWith(rigth_X, 1e-8), "NoPrec");

            }
        }

        [TestMethod]
        [TestCategory("Jacobi")]
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

                ISolver s = new Jacobi();
                IVector x1 = s.Solve(prec, mar, b, x0, 1e-10, 10000, logger);
               Assert.IsTrue(x1.CompareWith(rigth_X, 1e-8), "NoPrec");

            }
        }

        [TestMethod]
        [TestCategory("Jacobi")]
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

                ISolver s = new Jacobi();
                IVector x1 = s.Solve(prec, mar, b, x0, 1e-10, 10000, logger);
                Assert.IsTrue(x1.CompareWith(rigth_X, 1e-8), "NoPrec");

            }
        }

        [TestMethod]
        [TestCategory("Jacobi")]
        public void FirstColumn()
        {
            using (FileLogger logger = new FileLogger())
            {
                double[,] val = new double[2, 2] { { 1, 0 }, { 1, 0 } };
                IMatrix mar = new DenseMatrix(val);
                double[] valB = new double[] { 1, 1 };
                double[] valX = new double[] { 1, 0 };
                IPreconditioner prec = new NoPreconditioner();
                IVector b = new SimpleVector(valB);
                IVector x0 = new SimpleVector(2);
                IVector rigth_X = new SimpleVector(valX);

                ISolver s = new Jacobi();
                IVector x1 = s.Solve(prec, mar, b, x0, 1e-10, 10000, logger);
                Assert.IsTrue(x1[0] == rigth_X[0], "NoPrec");
            }
        }
    }
}
