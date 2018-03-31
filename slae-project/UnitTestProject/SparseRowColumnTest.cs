using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using slae_project.Matrix;
using slae_project.Vector.VectorExceptions;
using slae_project.Vector;
using slae_project.Preconditioner;

namespace UnitTestProject
{
    [TestClass]
    public class SparseRowColumnTest
    {
        [TestMethod]
        public void TestSPRC_isnotnull()
        {
            (int, int)[] coord = new(int, int)[11];
            coord[0] = (0, 0);
            coord[1] = (0, 2);
            coord[2] = (0, 3);
            coord[3] = (1, 1);
            coord[4] = (1, 3);
            coord[5] = (2, 2);
            coord[6] = (3, 1);
            coord[7] = (3, 3);
            coord[8] = (4, 1);
            coord[9] = (4, 2);
            coord[10] = (4, 4);

            double[] val = new double[11] { 1, 3, 4, 2, 5, 3, 1, 4, 2, 3, 5 };

            IMatrix c_matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)c_matr);
            Assert.IsNotNull(s_matr);
        }

        [TestMethod] 
        public void TestSPRC_Mult()
        {

            //double[] di = new double[8] { 3, 5, 6, 5, 7, 9, 11, 11 };
            //int[] ig = new int[9] { 1, 1, 2, 3, 3, 5, 7, 8, 9 }; //Индексы начала строк
            //int[] jg = new int[8] { 1, 1, 2, 4, 3, 5, 6, 5 }; //Номера столбцов для элементов
            //double[] al = new double[8] { -1, 1, 3, 3, -2, 2, 1, 4 }; //Элементы нижней диагонали
            //double[] au = new double[8] { 2, 8, 10, 12, -7, 3, 4, 1 }; //Элементы верхней диагонал

            (int, int)[] coord = new(int, int)[64];
            double[] val = new double[64] { 3, 2, 8, 0, 0, 0, 0, 0,
                                           -1, 5, 0, 0, 10, 0, 0, 0,
                                            1, 0, 6, 0, 0, -7, 0, 0,
                                            0, 0, 0, 5, 12, 0, 0, 0,
                                            0, 3, 0, 3, 7, 3, 0, 1,
                                            0, 0, -2, 0, 2, 9, 4, 0,
                                            0, 0, 0, 0, 0, 1, 11, 0,
                                            0, 0, 0, 0, 4, 0, 0, 11 };


            for (int i = 0; i < 64; i++)
            {
                coord[i] = (i / 8, i % 8);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);

            IVector x = new SimpleVector(new double[8] { 1, 2, 3, 4, 5, 6, 7, 8 });

            IVector result = s_matr.Mult(x);
            IVector right = new SimpleVector(new double[8] { 31, 59, -23, 80, 79, 86, 83, 108 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }
        //плотную+

        [TestMethod]
        public void TestSPRC_SolveL_nonsimmetr_plotn()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] {1,2,3,4,
                                           5,6,7,8,
                                            9,1,11,12,
                                            4,2,3,2
                                            };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }


            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1, -3, 29, -1 });

            IVector result = pre.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void TestSPRC_SolveL_simmetr_plotn()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] {1,2,3,4,
                                           2,5,6,7,
                                            3,6,10,4,
                                            4,7,4,15
                                            };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1, 4, 6, -286 });

            IVector result = pre.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void TestSPRC_SolveL_simmetr_notplotn()
        {
            (int, int)[] coord = new(int, int)[12];
            coord[0] = (0, 0);
            coord[1] = (0, 1);
            coord[2] = (1, 0);
            coord[3] = (1, 1);
            coord[4] = (1, 2);
            coord[5] = (1, 3);
            coord[6] = (2, 1);
            coord[7] = (2, 2);
            coord[8] = (2, 3);
            coord[9] = (3, 1);
            coord[10] = (3, 2);
            coord[11] = (3, 3);

            double[] val = new double[12] { 100, -10, -10, 2, -1, -1, -1, 101, 1, -1, 1, 2 };
            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 90, -10, 101, 2 });

            IVector result = pre.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 0.9, -1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }
       
         [TestMethod]
        public void TestSPRC_SolveL_nonsimmetr_notplotn()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] {1,2,0,0,
                                           5,6,7,8,
                                            9,10,11,12,
                                            0,0,15,16
                                            };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }


            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = pre.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 0.75, 0, -1 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }
        
        //vse notplotn
        [TestMethod]
        public void TestSPRC_SolveU_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] {100,-10,0,0,
                                           -10,2,-1,-1,
                                            0,-1,101,1,
                                            0,-1,1,2
                                            };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }
            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 0.8, -5, 3, 4 });

            IVector result = pre.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void TestSPRC_SolveU_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] {100,-10,0,0,
                                           -10,2,-1,-1,
                                            0,-1,101,1,
                                            0,3,4,2
                                            };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 0.8, -5, 3, 4 });

            IVector result = pre.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void TestSPRC_MultL_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] {100,-10,0,0,
                                           -10,2,-1,-1,
                                            0,-1,101,1,
                                            0,-1,1,2
                                            };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }


            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 0.9, -1, 1, 1 });

            IVector result = pre.MultL(x);
            IVector right = new SimpleVector(new double[4] { 90, -10, 101, 2 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void TestSPRC_MultL_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] {100,-10,0,0,
                                           -10,2,-1,-1,
                                            0,-1,101,1,
                                            0,3,4,2
                                            };


            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }


            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1,2, 3,4 });

            IVector result = pre.MultL(x);
            IVector right = new SimpleVector(new double[4] { 100, -8, 298, 47 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void TestSPRC_MultU_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] {100,-10,0,0,
                                           -10,2,-1,-1,
                                            0,-1,101,1,
                                            0,-1,1,2
                                            };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }


            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] {1, 2, 3, 4 });

            IVector result = pre.MultU(x);
            IVector right = new SimpleVector(new double[4] { 0.8, -5, 3, 4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void TestSPRC_MultU_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] {100,-10,0,0,
                                           -10,2,-1,-1,
                                            0,-1,101,1,
                                            0,3,4,2
                                            };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }


            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = pre.MultU(x);
            IVector right = new SimpleVector(new double[4] { 0.8, -5, 3, 4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void TestSPRC_MultT_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] {100,-10,0,0,
                                           -10,2,-1,-1,
                                            0,-1,101,1,
                                            0,-1,1,2
                                            };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }


            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = s_matr.T.Mult(x);
            IVector right = new SimpleVector(new double[4] { 80, -13, 305, 9 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void TestSPRC_MultT_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] {100,-10,0,0,
                                           -10,2,-1,-1,
                                            0,-1,101,1,
                                            0,3,4,2
                                            };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }


            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);

            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = s_matr.T.Mult(x);
            IVector right = new SimpleVector(new double[4] { 80, 3, 317, 9 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }
       //+notplotn
        
        //[TestMethod]
        //public void TestSPRC_MultLT_simmetr()
        //{
        //    (int, int)[] coord = new(int, int)[16];
        //    double[] val = new double[16] {100,-10,0,0,
        //                                   -10,2,-1,-1,
        //                                    0,-1,101,1,
        //                                    0,-1,1,2
        //                                    };

        //    for (int i = 0; i < 16; i++)
        //    {
        //        coord[i] = (i / 4, i % 4);
        //    }


        //    IMatrix matr = new CoordinateMatrix(coord, val);
        //    IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
        //    IPreconditioner pre = new LUPreconditioner(s_matr);
        //    IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

        //    IVector result = pre.T.MultL(x);
        //    IVector right = new SimpleVector(new double[4] { 80, -5, 300, 4});

        //    Assert.IsTrue(result.CompareWith(right, 1e-5));
        //}
        //[TestMethod]
        //public void TestSPRC_MultUT_simmetr()
        //{
        //    (int, int)[] coord = new(int, int)[16];
        //    double[] val = new double[16] {100,-10,0,0,
        //                                   -10,2,-1,-1,
        //                                    0,-1,101,1,
        //                                    0,-1,1,2
        //                                    };

        //    for (int i = 0; i < 16; i++)
        //    {
        //        coord[i] = (i / 4, i % 4);
        //    }


        //    IMatrix matr = new CoordinateMatrix(coord, val);
        //    IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
        //    IPreconditioner pre = new LUPreconditioner(s_matr);
        //    IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

        //    IVector result = pre.T.MultU(x);
        //    IVector right = new SimpleVector(new double[4] { 1, 1.9, 1, 2});

        //    Assert.IsTrue(result.CompareWith(right, 1e-5));
        //}
    }
}
