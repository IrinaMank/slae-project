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

        //Mult
        [TestMethod]
        public void TestSPRC_Mult_nonsimmetr_notplotn()
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
        [TestMethod]
        public void TestSPRC_Mult_nonsimmetr_plotn()
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

            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = s_matr.Mult(x);
            IVector right = new SimpleVector(new double[4] { 30, 70, 92, 25 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }
        [TestMethod]
        public void TestSPRC_Mult_simmetr_plotn()
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

            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = s_matr.Mult(x);
            IVector right = new SimpleVector(new double[4] { 30, 58, 61, 90 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }
        [TestMethod]
        public void TestSPRC_Mult_simmetr_notplotn()
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

            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = s_matr.Mult(x);
            IVector right = new SimpleVector(new double[4] { 80, -13, 305, 9 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        //SolveL
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

        //SolveU
        [TestMethod]
        public void TestSPRC_SolveU_simmetr_notplotn()
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
        public void TestSPRC_SolveU_nonsimmetr_notplotn()
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
        public void TestSPRC_SolveU_simmetr_plotn()
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
            IVector x = new SimpleVector(new double[4] { 30, -2, -29, 4 });

            IVector result = pre.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void TestSPRC_SolveU_nonsimmetr_plotn()
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
            IVector x = new SimpleVector(new double[4] { 30, 20, 9, 4 });

            IVector result = pre.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        //MultL
        [TestMethod]
        public void TestSPRC_MultL_simmetr_notplotn()
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
        public void TestSPRC_MultL_nonsimmetr_notplotn()
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

            IVector result = pre.MultL(x);
            IVector right = new SimpleVector(new double[4] { 100, -8, 298, 47 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }
        [TestMethod]
        public void TestSPRC_MultL_simmetr_plotn()
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
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = pre.MultL(x);
            IVector right = new SimpleVector(new double[4] { 1, 4, 6, -286 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void TestSPRC_MultL_nonsimmetr_plotn()
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
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = pre.MultL(x);
            IVector right = new SimpleVector(new double[4] { 1, -3, 29, -1 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        //MultU
        [TestMethod]
        public void TestSPRC_MultU_simmetr_notplotn()
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
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = pre.MultU(x);
            IVector right = new SimpleVector(new double[4] { 0.8, -5, 3, 4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }
        [TestMethod]
        public void TestSPRC_MultU_nonsimmetr_notplotn()
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
        public void TestSPRC_MultU_simmetr_plotn()
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
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = pre.MultU(x);
            IVector right = new SimpleVector(new double[4] { 30, -2, -29, 4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }
         [TestMethod]
        public void TestSPRC_MultU_nonsimmetr_plotn()
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
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = pre.MultU(x);
            IVector right = new SimpleVector(new double[4] { 30, 20, 9, 4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        //MultT
        [TestMethod]
        public void TestSPRC_MultT_simmetr_notplotn()
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
        public void TestSPRC_MultT_nonsimmetr_notplotn()
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
        [TestMethod]
        public void TestSPRC_MultT_simmetr_plotn()
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

            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = s_matr.T.Mult(x);
            IVector right = new SimpleVector(new double[4] { 30, 58, 61, 90 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));


        }
        [TestMethod]
        public void TestSPRC_MultT_nonsimmetr_plotn()
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
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = s_matr.T.Mult(x);
            IVector right = new SimpleVector(new double[4] { 54, 25, 62, 64 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }
        
        //SolveD
        [TestMethod]
        public void TestSPRC_SolveD_plotn()
        {
            (int, int)[] coord = new(int, int)[8];
            double[] val = new double[8] { 1.1, 2.2, 3.3, 4.4, 5.5, 6.6, 7.7, 8.8 };

            for (int i = 0; i < 8; i++)
            {
                coord[i] = (i, i);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
           
            IVector x = new SimpleVector(new double[8] { 1.65, 5.5, 4.95, 11, 8.25, 16.5, 11.55, 22 });
            
            IVector result = s_matr.SolveD(x);
            IVector right = new SimpleVector(new double[8] { 1.5, 2.5, 1.5, 2.5, 1.5, 2.5, 1.5, 2.5 });
            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }
        //SolveLT
        [TestMethod]
        public void TestSPRC_SolveLT_nonsimmetr_plotn()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 10, 9, 7, 4 });


            IVector result = pre.T.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 3, 4 });
            Assert.IsTrue(result.CompareWith(right, 0));

        }
        [TestMethod]
        public void TestSPRC_SolveLT_simmetr_plotn()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 36.3, 46.75, 22.55, 19.8 });


            IVector result = pre.T.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });
            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }
        [TestMethod]
        public void TestSPRC_SolveLT_nonsimmetr_notplotn()
        {
            (int, int)[] coord = new(int, int)[11];
            double[] val = new double[11] { 1.5, 1.1, 2.2, 2.5, 3.3, 4.4, 5.5, 3.5, 6.6, 7.7, 4.4 };

            for (int i = 0; i < 3; i++)
            {
                coord[i] = (i / 4, i % 4);
            }
            coord[3] = (1, 1);
            coord[4] = (1, 2);
            for (int i = 5; i < 8; i++)
            {
                coord[i] = ((i + 3) / 4, (i + 3) % 4);
            }
            for (int i = 8; i < 11; i++)
            {
                coord[i] = ((i + 5) / 4, (i + 5) % 4);
            }


            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 5.9, 14.6, 11.2, 4.4 });

            IVector result = pre.T.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }
        [TestMethod]
        public void TestSPRC_SolveLT_simmetr_notplotn()
        {
            (int, int)[] coord = new(int, int)[12];
            double[] val = new double[12] { 1, 5, 2, 7, 10, 5, 7, 3, 9, 10, 9, 4 };

            for (int i = 2; i < 9; i++)
            {
                coord[i] = ((i + 3) / 4, (i + 3) % 4);
            }
            coord[0] = (0, 0);
            coord[1] = (0, 2);
            coord[9] = (3, 1);
            coord[10] = (3, 2);
            coord[11] = (3, 3);


            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 6, 19, 12, 4 });

            IVector result = pre.T.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 0));
        }

        //SolveUt
        [TestMethod]
        public void TestSPRC_SolveUT_nonsimmetr_plotn()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1, 5, 8, 10 });

            IVector result = pre.T.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            Assert.IsTrue(result.CompareWith(right, 0));

        }
        [TestMethod]
        public void TestSPRC_SolveUT_simmetr_plotn()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1.65, 8.25, 25.3, 45.65 });

            IVector result = pre.T.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });
            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }
        [TestMethod]
        public void TestSPRC_SolveUT_nonsimmetr_notplotn()
        {
            (int, int)[] coord = new(int, int)[11];
            double[] val = new double[11] { 1.5, 1.1, 2.2, 2.5, 3.3, 4.4, 5.5, 3.5, 6.6, 7.7, 4.4 };

            for (int i = 0; i < 3; i++)
            {
                coord[i] = (i / 4, i % 4);
            }
            coord[3] = (1, 1);
            coord[4] = (1, 2);
            for (int i = 5; i < 8; i++)
            {
                coord[i] = ((i + 3) / 4, (i + 3) % 4);
            }
            for (int i = 8; i < 11; i++)
            {
                coord[i] = ((i + 5) / 4, (i + 5) % 4);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1.5, 3.6, 9, 4.4 });

            IVector result = pre.T.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }
        [TestMethod]
        public void TestSPRC_SolveUT_simmetr_notplotn()
        {
            (int, int)[] coord = new(int, int)[12];
            double[] val = new double[12] { 1, 5, 2, 7, 10, 5, 7, 3, 9, 10, 9, 4 };

            for (int i = 2; i < 9; i++)
            {
                coord[i] = ((i + 3) / 4, (i + 3) % 4);
            }
            coord[0] = (0, 0);
            coord[1] = (0, 2);
            coord[9] = (3, 1);
            coord[10] = (3, 2);
            coord[11] = (3, 3);

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1, 2, 15, 23 });

            IVector result = pre.T.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            Assert.IsTrue(result.CompareWith(right, 0));
        }

        //MultD
        [TestMethod]
        public void TestSPRC_MultD_plotn()
        {
            (int, int)[] coord = new(int, int)[8];
            double[] val = new double[8] { 1.1, 2.2, 3.3, 4.4, 5.5, 6.6, 7.7, 8.8 };

            for (int i = 0; i < 8; i++)
            {
                coord[i] = (i, i);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IVector x = new SimpleVector(new double[8] { 1.5, 2.5, 1.5, 2.5, 1.5, 2.5, 1.5, 2.5 });

            IVector result = s_matr.MultD(x);
            IVector right = new SimpleVector(new double[8] { 1.65, 5.5, 4.95, 11, 8.25, 16.5, 11.55, 22 });
            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        //MultUT,MultLT
        //не сработал notplotn SolveD
        [TestMethod]
        public void TestSPRC_SolveD_notplotn()
        {
            (int, int)[] coord = new(int, int)[6];
            double[] val = new double[6] { 1.1, 3.3, 4.4, 5.5, 7.7, 8.8 };

            coord[0] = (0, 0);
            for (int i = 1; i < 4; i++)
            {
                coord[i] = (i + 1, i + 1);
            }
            coord[4] = (6, 6);
            coord[5] = (7, 7);

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IVector x = new SimpleVector(new double[8] { 1.65, 0, 4.95, 6.6, 8.25, 0, 11.55, 13.2 });


            IVector result = s_matr.SolveD(x);
            IVector right = new SimpleVector(new double[8] { 1.5, 1.5, 1.5, 1.5, 1.5, 1.5, 1.5, 1.5 });
            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }

        //MultLT
        [TestMethod]
        public void TestSPRC_MultLT_simmetr_notplotn()
        {
            (int, int)[] coord = new(int, int)[12];
            double[] val = new double[12] { 1, 5, 2, 7, 10, 5, 7, 3, 9, 10, 9, 4 };

            for (int i = 2; i < 9; i++)
            {
                coord[i] = ((i + 3) / 4, (i + 3) % 4);
            }
            coord[0] = (0, 0);
            coord[1] = (0, 2);
            coord[9] = (3, 1);
            coord[10] = (3, 2);
            coord[11] = (3, 3);

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);

            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = pre.T.MultL(x);
            IVector right = new SimpleVector(new double[4] { 6, 19, 12, 4 });

            Assert.IsTrue(result.CompareWith(right, 0));
        }
        [TestMethod]
        public void TestSPRC_MultLT_simmetr_plotn()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });

            IVector result = pre.T.MultL(x);
            IVector right = new SimpleVector(new double[4] { 36.3, 46.75, 22.55, 19.8 });
            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }
        [TestMethod]
        public void TestSPRC_MultLT_nonsimmetr_notplotn()
        {
            (int, int)[] coord = new(int, int)[11];
            double[] val = new double[11] { 1.5, 1.1, 2.2, 2.5, 3.3, 4.4, 5.5, 3.5, 6.6, 7.7, 4.4 };

            for (int i = 0; i < 3; i++)
            {
                coord[i] = (i / 4, i % 4);
            }
            coord[3] = (1, 1);
            coord[4] = (1, 2);
            for (int i = 5; i < 8; i++)
            {
                coord[i] = ((i + 3) / 4, (i + 3) % 4);
            }
            for (int i = 8; i < 11; i++)
            {
                coord[i] = ((i + 5) / 4, (i + 5) % 4);
            }


            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = pre.T.MultL(x);
            IVector right = new SimpleVector(new double[4] { 5.9, 14.6, 11.2, 4.4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }
        [TestMethod]
        public void TestSPRC_MultLT_nonsimmetr_plotn()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });


            IVector result = pre.T.MultL(x);
            IVector right = new SimpleVector(new double[4] { 10, 9, 7, 4 });
            Assert.IsTrue(result.CompareWith(right, 0));
        }


        //MultUT
        [TestMethod]
        public void TestSPRC_MultUT_simmetr_notplotn()
        {
            (int, int)[] coord = new(int, int)[12];
            double[] val = new double[12] { 1, 5, 2, 7, 10, 5, 7, 3, 9, 10, 9, 4 };

            for (int i = 2; i < 9; i++)
            {
                coord[i] = ((i + 3) / 4, (i + 3) % 4);
            }
            coord[0] = (0, 0);
            coord[1] = (0, 2);
            coord[9] = (3, 1);
            coord[10] = (3, 2);
            coord[11] = (3, 3);

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = pre.T.MultU(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 15, 23 });

            Assert.IsTrue(result.CompareWith(right, 0));

        }
        [TestMethod]
        public void TestSPRC_MultUT_simmetr_plotn()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });

            IVector result = pre.T.MultU(x);
            IVector right = new SimpleVector(new double[4] { 1.65, 8.25, 25.3, 45.65 });
            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }
        [TestMethod]
        public void TestSPRC_MultUT_nonsimmetr_notplotn()
        {
            (int, int)[] coord = new(int, int)[11];
            double[] val = new double[11] { 1.5, 1.1, 2.2, 2.5, 3.3, 4.4, 5.5, 3.5, 6.6, 7.7, 4.4 };

            for (int i = 0; i < 3; i++)
            {
                coord[i] = (i / 4, i % 4);
            }
            coord[3] = (1, 1);
            coord[4] = (1, 2);
            for (int i = 5; i < 8; i++)
            {
                coord[i] = ((i + 3) / 4, (i + 3) % 4);
            }
            for (int i = 8; i < 11; i++)
            {
                coord[i] = ((i + 5) / 4, (i + 5) % 4);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = pre.T.MultU(x);
            IVector right = new SimpleVector(new double[4] { 1.5, 3.6, 9, 4.4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }
        [TestMethod]
        public void TestSPRC_MultUT_nonsimmetr_plotn()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IPreconditioner pre = new LUPreconditioner(s_matr);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = pre.T.MultU(x);
            IVector right = new SimpleVector(new double[4] { 1, 5, 8, 10 });
            Assert.IsTrue(result.CompareWith(right, 0));
        }

        //bigmatrix

        [TestMethod]
        public void TestSPRC_Mult_big_matrix()
        {
            (int, int)[] coord = new(int, int)[10000];
            double[] val = new double[10000];

            for (int i = 0; i < 10000; i++)
            {
                coord[i] = (i / 100, i % 100);
                val[i] = 1;
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IVector x = new SimpleVector(new double[100]);
            IVector right = new SimpleVector(new double[100]);
            for (int i = 0; i < 100; i++)
            {
                x[i] = 1;
                right[i] = 100;
            }

            
            IVector result = s_matr.Mult(x);
            
            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }
        [TestMethod]
        public void TestSPRC_SolveL_big_matrix()
        {
            (int, int)[] coord = new(int, int)[10000];
            double[] val = new double[10000];

            for (int i = 0; i < 10000; i++)
            {
                coord[i] = (i / 100, i % 100);
                val[i] = 1;
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IVector x = new SimpleVector(new double[100]);
            IVector right = new SimpleVector(new double[100]);
            for (int i = 0; i < 100; i++)
            {
                x[i] = i + 1;
                right[i] = 1;
            }

            
            IVector result = s_matr.SolveL(x);
            Assert.IsTrue(result.CompareWith(right, 1e-5));


        }
        [TestMethod]
        public void TestSPRC_SolveU_big_matrix()
        {
            (int, int)[] coord = new(int, int)[10000];
            double[] val = new double[10000];

            for (int i = 0; i < 10000; i++)
            {
                coord[i] = (i / 100, i % 100);
                val[i] = 1;
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IVector x = new SimpleVector(new double[100]);
            IVector right = new SimpleVector(new double[100]);
            for (int i = 99; i >= 0; i--)
            {
                x[i] = 100 - i;
                right[i] = 1;
            }

            IVector result = s_matr.SolveU(x);
            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }
        [TestMethod]
        public void TestSPRC_MultL_big_matrix()
        {
            (int, int)[] coord = new(int, int)[10000];
            double[] val = new double[10000];

            for (int i = 0; i < 10000; i++)
            {
                coord[i] = (i / 100, i % 100);
                val[i] = 1;
            }

            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IVector x = new SimpleVector(new double[100]);
            IVector right = new SimpleVector(new double[100]);
            for (int i = 0; i < 100; i++)
            {
                x[i] = 1;
                right[i] = i + 1;
            }

            IVector result = s_matr.MultL(x);
            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }
        //[TestMethod]
        //public void TestSPRC_MultU_big_matrix()
        //{
        //    (int, int)[] coord = new(int, int)[10000];
        //    double[] val = new double[10000];

        //    for (int i = 0; i < 10000; i++)
        //    {
        //        coord[i] = (i / 100, i % 100);
        //        val[i] = 1;
        //    }

        //    IMatrix matr = new CoordinateMatrix(coord, val);
        //    IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
        //   IPreconditioner pre = new LUPreconditioner(s_matr);
        //    IVector x = new SimpleVector(new double[100]);
        //    IVector right = new SimpleVector(new double[100]);
        //    for (int i = 99; i >= 0; i--)
        //    {
        //        x[i] = 1;
        //        right[i] = 100 - i;
        //    }

        //    IVector result = pre.MultL(x);
        //    Assert.IsTrue(result.CompareWith(right, 1e-5));
        //}
        //[TestMethod]
        //public void TestSPRC_MultT_big_matrix()
        //{
        //    (int, int)[] coord = new(int, int)[10000];
        //    double[] val = new double[10000];

        //    for (int i = 0; i < 10000; i++)
        //    {
        //        coord[i] = (i / 100, i % 100);
        //        val[i] = 1;
        //    }

        //    IMatrix matr = new CoordinateMatrix(coord, val);
        //    IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
        //    IPreconditioner pre = new LUPreconditioner(s_matr);
        //    IVector x = new SimpleVector(new double[100]);
        //    IVector right = new SimpleVector(new double[100]);
        //    for (int i = 0; i < 100; i++)
        //    {
        //        x[i] = 1;
        //        right[i] = 100;
        //    }

        //    IVector result = pre.MultL(x);
        //    Assert.IsTrue(result.CompareWith(right, 1e-5));
        //}
        
        ////Тест с несовпадающими размерностями матрицы и вектора
        //[TestMethod]
        //public void TestSPRC_Mult_rasmern_ne_sovpadaet()
        //{
        //    (int, int)[] coord = new(int, int)[16];
        //    double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

        //    for (int i = 0; i < 16; i++)
        //    {
        //        coord[i] = (i / 4, i % 4);
        //    }

        //    IMatrix matr = new CoordinateMatrix(coord, val);
        //    IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
        //    IVector x = new SimpleVector(new double[3] { 1, 2, 3 });

        //    IVector result;
        //    Assert.ThrowsException<DifferentSizeException>(() => { result = s_matr.Mult(x); });
        //}
    }
}
