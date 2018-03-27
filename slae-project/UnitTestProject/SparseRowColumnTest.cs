using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using slae_project.Matrix;
using slae_project.Vector.VectorExceptions;
using slae_project.Vector;

namespace UnitTestProject
{
    [TestClass]
    public class SparseRowColumnTest
    {
        [TestMethod] 
        public void TestSPRC()
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

        // тот самый localtest (работает)
        [TestMethod]
        public void TestMatrix_coord_in_sparserowcolumn()
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
        public void Test_SolveL_nonsimmetr()
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


            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IVector x = new SimpleVector(new double[5] { 1, 2, 3, 4, 5 });

            IVector result = s_matr.SolveU(x);
            IVector right = new SimpleVector(new double[5] { -6, -1.5, 1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod] 
        public void Test_SolveU_nonsimmetr()
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


            IMatrix matr = new CoordinateMatrix(coord, val);
            IMatrix s_matr = new SparseRowColumnMatrix((CoordinateMatrix)matr);
            IVector x = new SimpleVector(new double[5] { 1, 2, 3, 4, 5 });

            IVector result = s_matr.SolveU(x);
            IVector right = new SimpleVector(new double[5] { -6, -1.5, 1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));

        }


    }
}
