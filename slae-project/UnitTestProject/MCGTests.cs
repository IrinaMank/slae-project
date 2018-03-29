using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using slae_project.Matrix;
using slae_project.Vector.VectorExceptions;
using slae_project.Vector;
using slae_project.Solver;

namespace UnitTestProject
{
    [TestClass]
    public class MCGTests
    {
        [TestMethod]
        public void PrimTest()
        {
            //(int, int)[] coord = new(int, int)[25];
            //double[] valMatrix = new double[25] { 1, 5, 1, 2, 1, 8, 2, 1, 3, 2, 2, 9, 3, 7, 3, 1, 3, 10, 4, 6, 3, 1, 2, 11, 5 };
            //double[] valB = new double[] { 27, 37, 72, 83, 80 };
            //double[] valX = new double[] { 1, 2, 3, 4, 5 };

            //for (int i = 0; i < 25; i++)
            //{
            //    coord[i] = (i / 5, i % 5);
            //}

            //(int, int)[] coord = new(int, int)[9];
            //double[] valMatrix = new double[9] { 1, 0, 0, 0, 2, 0, 0, 0, 3 };
            //double[] valB = new double[] { 1, 2, 3 };
            //double[] valX = new double[] { 1, 1, 1 };

            //for (int i = 0; i < 9; i++)
            //{
            //    coord[i] = (i / 3, i % 3);
            //}

            (int, int)[] coord = new(int, int)[100];
            double[] valMatrix = new double[100] { 7,2,0,0,0,1,3,0,0,0,1,8,4,0,0,0,1,2,0,0,0,3,14,3,0,0,0,4,4,0,0,0,2,9,1,0,0,0,2,4,0,0,0,4,6,1,0,0,0,1,2,0,0,0,1,5,2,0,0,0,2,3,0,0,0,3,11,3,0,0,0,4,1,0,0,0,3,12,4,0,0,0,1,4,0,0,0,2,8,1,0,0,0,2,1,0,0,0,1,4 };
            double[] valB = new double[] { 38, 52, 128, 105, 62, 51, 127, 164, 117, 62 };
            double[] valX = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

            for (int i = 0; i < 100; i++)
            {
                coord[i] = (i / 10, i % 10);
            }

            IMatrix mar = new CoordinateMatrix(coord, valMatrix);
            
            IVector b = new SimpleVector(valB);
            //IVector x0 = new SimpleVector(5);
            IVector x0 = new SimpleVector(10);
            IVector rigth_X = new SimpleVector(valX);

            MSGSolver s = new MSGSolver();
            IVector x = s.Solve(mar, b, x0, 1e-8, 10000);

            Assert.IsTrue(x.CompareWith(rigth_X, 1e-6));
        }
    }
}