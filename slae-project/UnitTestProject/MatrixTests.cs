using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using slae_project.Matrix;
using slae_project.Matrix.MatrixExceptions;
using slae_project.Vector.VectorExceptions;
using slae_project.Vector;
//вывод объектов загуглить
namespace UnitTestProject
{
    [TestClass]
    public class CoordinateMatrixTests
    {
        //private TestContext testContextInstance;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        //public TestContext TestContext
        //{
        //    get { return testContextInstance; }
        //    set { testContextInstance = value; }
        //}

        //плотная матрица
        [TestMethod]
        public void Test_f_Mult_plot_format()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = mar.Mult(x);
            IVector right = new SimpleVector(new double[4] { 37, 24, 14, 10 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        //матрица с нулями
        [TestMethod]
        public void Test_f_Mult_ne_plot_format()
        {
            (int, int)[] coord = new(int, int)[14];
            double[] val = new double[14] { 1, 2, 3, 4, 5, 6, 7, 8, 23, 52, 6, 9, 34, 5 };

            for (int i = 0; i < 8; i++)
            {
                coord[i] = (i / 4, i % 4);
            }
            //кусочек говнокода
            coord[8] = (2, 0);
            coord[9] = (2, 2);
            coord[10] = (2, 3);
            coord[11] = (3, 0);
            coord[12] = (3, 1);
            coord[13] = (3, 3);

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = mar.Mult(x);
            IVector right = new SimpleVector(new double[4] { 30, 70, 203, 97 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        //Тест с несовпадающими размерностями матрицы и вектора
        [TestMethod]
        public void Test_f_Mult_rasmern_ne_sovpadaet()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[3] { 1, 2, 3 });

            IVector result;
            Assert.ThrowsException<DifferentSizeException>(() => { result = mar.Mult(x); });
        }

        [TestMethod]
        public void Test_f_SolveL()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });
            IVector y;

            y = mar.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));
            //13, 0.5, 0.5, -4
            //5, 0, 0, -1
        }

        [TestMethod]
        public void Test_f_MultL()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            IVector y;

            y = mar.MultL(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 3, 4 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_MultL_del_na_0()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[15] { 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 1; i < 16; i++)
            {
                coord[i - 1] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            IVector y;
            Assert.ThrowsException<LUFailException>(() => { y = mar.MultL(x); });
        }

        [TestMethod]
        public void Test_f_SolveL_SolveU()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector z = mar.Mult(x);
            z = mar.SolveL(x);
            z = mar.SolveU(z);
            IVector right = new SimpleVector(new double[4] { 5, 0, 0, -1 });
            Assert.IsTrue(z.CompareWith(right, 1e-5));
        }


    }
}
