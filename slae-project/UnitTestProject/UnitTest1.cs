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
    }
}

