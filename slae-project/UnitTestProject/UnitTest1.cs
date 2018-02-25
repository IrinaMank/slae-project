using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using slae_project.Matrix;
using slae_project.Vector;
namespace UnitTestProject
    {
        [TestClass]
        public class UnitTest1
        {
            [TestMethod]
            public void TestMethod1()
            {
                double[] x = new double[] { 1, 2, 3 };
                IVector v = new SimpleVector(x);
                Assert.IsNotNull(v);
            }
    }
    }

