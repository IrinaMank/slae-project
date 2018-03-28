using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using slae_project.Vector;
using slae_project.Vector.VectorExceptions;
namespace UnitTestProject
{
    [TestClass]
    public class VectorUnitTest
    {
        [TestMethod]
        public void createVector()
        {
            double[] x = new double[] { 1, 2, 3 };
            IVector v = new SimpleVector(x);
            Assert.IsNotNull(v);
        }

        [TestMethod]
        public void normVector()
        {
            double[] x = new double[] { 1, 2, 3 };
            IVector v = new SimpleVector(x);
            Assert.AreEqual(v.Norm, Math.Sqrt(14));
        }

        [TestMethod]
        public void wrongSize()
        {
            Assert.ThrowsException<WrongSizeException>(() =>
           {
               IVector v = new SimpleVector(-10);
           }
                );

        }

        [TestMethod]
        public void addVector()
        {
            double[] x = new double[] { 1, 2, 3 };
            double[] xx = new double[] { 2, 4, 6 };
            IVector v = new SimpleVector(x);
            IVector resultRight = v.Add(v, 1, 1);
            IVector result = new SimpleVector(xx);
            Assert.IsTrue(result.CompareWith(resultRight, 1e-9));
        }

        [TestMethod]
        public void multVector()
        {
            double[] x = new double[] { 1, 2, 3 };
            IVector v = new SimpleVector(x);
            double resultRight = 14;
            double result = v.ScalarMult(v);
            Assert.IsTrue(result==resultRight);
        }

    }
}

