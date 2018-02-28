using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using slae_project.Matrix;
using slae_project.Vector.VectorExceptions;
using slae_project.Vector;
//TODO: операция сравнения векторов
//вывод объектов загуглить
namespace UnitTestProject
{
    [TestClass]
    public class MatrixTests
    {
        private TestContext testContextInstance;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        [TestMethod]
        public void multVectorMatrix()
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
            IVector right = new SimpleVector(new double[4] { 37, 24, 14, 10});

            //Assert.AreEqual(result, right);

            IVector x1 = new SimpleVector(1);
            IVector x2 = new SimpleVector(1);
            Assert.IsTrue(x1 == x2);
            //Assert.Equals(x1, x2);

        }
    }
}
