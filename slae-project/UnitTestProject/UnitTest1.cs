﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using slae_project.Matrix;
    namespace UnitTestProject
    {
        [TestClass]
        public class UnitTest1
        {
            [TestMethod]
            public void TestMethod1()
            {
                double[] x = new double[] { 1, 2, 3 };
                Vector v = new Vector(x);
                Assert.IsNotNull(v);
        }
        }
    }

