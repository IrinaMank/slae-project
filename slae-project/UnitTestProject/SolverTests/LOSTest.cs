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

                ISolver s = new LOSSolver();
                IVector x = s.Solve(prec, mar, b, x0, 1e-8, 10000, logger);

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
                IVector x1 = s.Solve(prec, mar, b, x01, 1e-8, 10000, logger);
                IVector x2 = s.Solve(prec, mar, b, x02, 1e-8, 10000, logger);
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
                IVector x = s.Solve(prec, mar, b, x0, 1e-2, 10000, logger);

                Assert.IsTrue(x.CompareWith(rigth_X, 1e-2));
            }
        }

        //[TestMethod]
        //public void ShittyMatrix()
        //{
        //    using (FileLogger logger = new FileLogger())
        //    {
        //        (int, int)[] coord = new(int, int)[9];
        //        double[] valMatrix = new double[9] { -1, 2, 3, -1, -1, 4, -4, 7, 9 };
        //        double[] valB = new double[] { 12, 9, 37 };
        //        double[] valX = new double[] { 1, 2, 3 };

        //        coord[0] = (0, 0);
        //        coord[1] = (0, 1);
        //        coord[2] = (0, 2);
        //        coord[3] = (1, 0);
        //        coord[4] = (1, 1);
        //        coord[5] = (1, 2);
        //        coord[6] = (2, 0);
        //        coord[7] = (2, 1);
        //        coord[8] = (2, 2);

        //        IMatrix mar = new CoordinateMatrix(coord, valMatrix);

        //        IPreconditioner precNo = new NoPreconditioner();
        //        IPreconditioner precLU = new LUPreconditioner(mar);
        //        IPreconditioner precDi = new DiagonalPreconditioner(mar);

        //        IVector b = new SimpleVector(valB);
        //        IVector x0 = new SimpleVector(3);
        //        IVector rigth_X = new SimpleVector(valX);

        //        ISolver s = new LOSSolver();
        //        IVector x = s.Solve(precNo, mar, b, x0, 1e-10, 10000, logger);
        //        Assert.IsTrue(x.CompareWith(rigth_X, 1e-10), "No prec");

        //        IVector x2 = s.Solve(precLU, mar, b, x0, 1e-4, 10000, logger);
        //        Assert.IsTrue(x2.CompareWith(rigth_X, 1e-4), "LU prec");

        //        IVector x3 = s.Solve(precDi, mar, b, x0, 1e-4, 10000, logger);
        //        Assert.IsTrue(x3.CompareWith(rigth_X, 1e-4), "Di prec");
        //    }
        //}

        [TestMethod]
        public void BadObuslMatrix()
        {
            using (FileLogger logger = new FileLogger())
            {
                double[,] val = new double[3, 3] { { 2,2,5 }, { 3,1,4}, { 4,1,8 } };

                IMatrix mar = new DenseMatrix(val);
                double[] valB = new double[] { 21,17,30 };
                double[] valX = new double[] { 1, 2, 3};

                IPreconditioner precNo = new NoPreconditioner();
                IPreconditioner precLU = new LUPreconditioner(mar);
                IPreconditioner precDi = new DiagonalPreconditioner(mar);

                IVector b = new SimpleVector(valB);
                IVector x0 = new SimpleVector(3);
                IVector rigth_X = new SimpleVector(valX);

                ISolver s = new LOSSolver();
                IVector x = s.Solve(precNo, mar, b, x0, 1e-10, 10000, logger);
                Assert.IsTrue(x.CompareWith(rigth_X, 1e-10), "No prec");

                IVector x2 = s.Solve(precLU, mar, b, x0, 1e-4, 10000, logger);
                Assert.IsTrue(x2.CompareWith(rigth_X, 1e-4), "LU prec");

                IVector x3 = s.Solve(precDi, mar, b, x0, 1e-4, 10000, logger);
                Assert.IsTrue(x3.CompareWith(rigth_X, 1e-4), "Di prec");
            }
        }
    }
}