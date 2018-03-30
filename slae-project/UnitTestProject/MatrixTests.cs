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

        //////////////////Mult#1


        //плотная несимметричная матрица
        [TestMethod]
        public void Test_f_Mult_plot_format_nonsimmetr()
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

            Assert.IsTrue(result.CompareWith(right, 0));
        }

        [TestMethod]
        public void Test_f_Mult_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });
            IVector y;

            y = mar.Mult(x);
            IVector right = new SimpleVector(new double[4] { 36.3, 49.5, 36.3, 45.65 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_Mult_ne_plot_nonsimmetr()
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


            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = mar.Mult(x);
            IVector right = new SimpleVector(new double[4] { 4.8, 5.8, 13.4, 18.7 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_Mult_ne_plot_simmetr()
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

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = mar.Mult(x);
            IVector right = new SimpleVector(new double[4] { 16, 65, 64, 63 });

            Assert.IsTrue(result.CompareWith(right, 0));
        }

        [TestMethod]
        public void Test_f_Mult_big_matrix()
        {
            (int, int)[] coord = new(int, int)[10000];
            double[] val = new double[10000];

            for (int i = 0; i < 10000; i++)
            {
                coord[i] = (i / 100, i % 100);
                val[i] = 1;
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[100]);
            IVector right = new SimpleVector(new double[100]);
            for (int i = 0; i < 100; i++)
            {
                x[i] = 1;
                right[i] = 100;
            }

            IVector y;
            y = mar.Mult(x);
            Assert.IsTrue(y.CompareWith(right, 0));

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


        //////////////////SolveL#2


        [TestMethod]
        public void Test_f_SolveL_plot_format_nonsimmetr()
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
            Assert.IsTrue(y.CompareWith(right, 0));

        }

        [TestMethod]
        public void Test_f_SolveL_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1.65, 8.25, 25.3, 45.65 });
            IVector y;

            y = mar.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_SolveL_ne_plot_nonsimmetr()
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


            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1.5, 2.5, 13.4, 18.7 });

            IVector result = mar.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_SolveL_ne_plot_simmetr()
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

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 15, 23 });

            IVector result = mar.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 0));
        }

        [TestMethod]
        public void Test_f_SolveL_big_matrix()
        {
            (int, int)[] coord = new(int, int)[10000];
            double[] val = new double[10000];

            for (int i = 0; i < 10000; i++)
            {
                coord[i] = (i / 100, i % 100);
                val[i] = 1;
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[100]);
            IVector right = new SimpleVector(new double[100]);
            for (int i = 0; i < 100; i++)
            {
                x[i] = i + 1;
                right[i] = 1;
            }

            IVector y;
            y = mar.SolveL(x);
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        //[TestMethod]
        //public void Test_f_SolveL_Exception()
        //{
        //    (int, int)[] coord = new(int, int)[16];
        //    double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

        //    for (int i = 0; i < 16; i++)
        //    {
        //        coord[i] = (i / 4, i % 4);
        //    }

        //    IMatrix mar = new CoordinateMatrix(coord, val);
        //    IVector x = new SimpleVector(new double[4] { 0,0,0,0 });
        //    IVector y;            
        //    Assert.ThrowsException<SlaeNotCompatipableException>(() => { y = mar.SolveL(x); });
        //}


        //////////////////SolveU#3

        [TestMethod]
        public void Test_f_SolveU_plot_format_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 13, 7, 3, 1 });
            IVector y;

            y = mar.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            Assert.IsTrue(y.CompareWith(right, 0));

        }

        [TestMethod]
        public void Test_f_SolveU_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 36.3, 46.75, 22.55, 19.8 });
            IVector y;

            y = mar.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_SolveU_ne_plot_nonsimmetr()
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


            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 4.8, 5.8, 3.5, 4.4 });

            IVector result = mar.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_SolveU_ne_plot_simmetr()
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

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 6, 19, 12, 4 });

            IVector result = mar.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 0));
        }

        [TestMethod]
        public void Test_f_SolveU_big_matrix()
        {
            (int, int)[] coord = new(int, int)[10000];
            double[] val = new double[10000];

            for (int i = 0; i < 10000; i++)
            {
                coord[i] = (i / 100, i % 100);
                val[i] = 1;
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[100]);
            IVector right = new SimpleVector(new double[100]);
            for (int i = 99; i >= 0; i--)
            {
                x[i] = 100 - i;
                right[i] = 1;
            }

            IVector y;
            y = mar.SolveU(x);
            Assert.IsTrue(y.CompareWith(right, 1e-5));
        }



        //////////////////MultL#4


        [TestMethod]
        public void Test_f_MultL_plot_format_nonsimmetr()
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
            Assert.IsTrue(y.CompareWith(right, 0));

        }

        [TestMethod]
        public void Test_f_MultL_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });
            IVector y;

            y = mar.MultL(x);
            IVector right = new SimpleVector(new double[4] { 1.65, 8.25, 25.3, 45.65 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_MultL_ne_plot_nonsimmetr()
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


            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = mar.MultL(x);
            IVector right = new SimpleVector(new double[4] { 1.5, 2.5, 13.4, 18.7 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_MultL_ne_plot_simmetr()
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

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = mar.MultL(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 15, 23 });

            Assert.IsTrue(result.CompareWith(right, 0));
        }

        [TestMethod]
        public void Test_f_MultL_big_matrix()
        {
            (int, int)[] coord = new(int, int)[10000];
            double[] val = new double[10000];

            for (int i = 0; i < 10000; i++)
            {
                coord[i] = (i / 100, i % 100);
                val[i] = 1;
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[100]);
            IVector right = new SimpleVector(new double[100]);
            for (int i = 0; i < 100; i++)
            {
                x[i] = 1;
                right[i] = i + 1;
            }

            IVector y;
            y = mar.MultL(x);
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_MultL_Exception()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[3] { 1, 1, 1 });
            IVector y;
            Assert.ThrowsException<DifferentSizeException>(() => { y = mar.MultL(x); });

        }

        //////////////////MultU#5

        [TestMethod]
        public void Test_f_MultU_plot_format_nonsimmetr()
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

            y = mar.MultU(x);
            IVector right = new SimpleVector(new double[4] { 13, 7, 3, 1 });
            Assert.IsTrue(y.CompareWith(right, 0));

        }

        [TestMethod]
        public void Test_f_MultU_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });
            IVector y;

            y = mar.MultU(x);
            IVector right = new SimpleVector(new double[4] { 36.3, 46.75, 22.55, 19.8 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_MultU_ne_plot_nonsimmetr()
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


            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = mar.MultU(x);
            IVector right = new SimpleVector(new double[4] { 4.8, 5.8, 3.5, 4.4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_MultU_ne_plot_simmetr()
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

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = mar.MultU(x);
            IVector right = new SimpleVector(new double[4] { 6, 19, 12, 4 });

            Assert.IsTrue(result.CompareWith(right, 0));
        }

        [TestMethod]
        public void Test_f_MultU_big_matrix()
        {
            (int, int)[] coord = new(int, int)[10000];
            double[] val = new double[10000];

            for (int i = 0; i < 10000; i++)
            {
                coord[i] = (i / 100, i % 100);
                val[i] = 1;
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[100]);
            IVector right = new SimpleVector(new double[100]);
            for (int i = 99; i >= 0; i--)
            {
                x[i] = 1;
                right[i] = 100 - i;
            }

            IVector y;
            y = mar.MultU(x);
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_MultU_Exception()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[3] { 1, 1, 1 });
            IVector y;
            Assert.ThrowsException<DifferentSizeException>(() => { y = mar.MultU(x); });

        }


        //////////////////MultT#6


        //плотная несимметричная матрица
        [TestMethod]
        public void Test_f_MultT_plot_format_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 1, 1, 1, 4, 1, 1, 1, 4, 3, 1, 1, 4, 3, 2, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = mar.T.Mult(x);
            IVector right = new SimpleVector(new double[4] { 37, 24, 14, 10 });

            Assert.IsTrue(result.CompareWith(right, 0));
        }

        [TestMethod]
        public void Test_f_MultT_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });
            IVector y;

            y = mar.T.Mult(x);
            IVector right = new SimpleVector(new double[4] { 36.3, 49.5, 36.3, 45.65 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_MultT_ne_plot_nonsimmetr()
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


            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = mar.T.Mult(x);
            IVector right = new SimpleVector(new double[4] { 5.9, 15.7, 16.7, 4.4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_MultT_ne_plot_simmetr()
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

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = mar.T.Mult(x);
            IVector right = new SimpleVector(new double[4] { 16, 65, 64, 63 });

            Assert.IsTrue(result.CompareWith(right, 0));
        }

        [TestMethod]
        public void Test_f_MultT_big_matrix()
        {
            (int, int)[] coord = new(int, int)[10000];
            double[] val = new double[10000];

            for (int i = 0; i < 10000; i++)
            {
                coord[i] = (i / 100, i % 100);
                val[i] = 1;
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[100]);
            IVector right = new SimpleVector(new double[100]);
            for (int i = 0; i < 100; i++)
            {
                x[i] = 1;
                right[i] = 100;
            }

            IVector y;
            y = mar.T.Mult(x);
            Assert.IsTrue(y.CompareWith(right, 0));

        }

        //Тест с несовпадающими размерностями матрицы и вектора
        [TestMethod]
        public void Test_f_MultT_rasmern_ne_sovpadaet()
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
            Assert.ThrowsException<DifferentSizeException>(() => { result = mar.T.Mult(x); });
        }


        //////////////////SolveLT#7


        [TestMethod]
        public void Test_f_SolveLT_plot_format_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 10, 9, 7, 4 });
            IVector y;

            y = mar.T.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 3, 4 });
            Assert.IsTrue(y.CompareWith(right, 0));

        }

        [TestMethod]
        public void Test_f_SolveLT_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 36.3, 46.75, 22.55, 19.8 });
            IVector y;

            y = mar.T.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_SolveLT_ne_plot_nonsimmetr()
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


            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 5.9, 14.6, 11.2, 4.4 });

            IVector result = mar.T.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_SolveLT_ne_plot_simmetr()
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

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 6, 19, 12, 4 });

            IVector result = mar.T.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 0));
        }


        //////////////////SolveUT#8

        [TestMethod]
        public void Test_f_SolveUT_plot_format_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 5, 8, 10 });
            IVector y;

            y = mar.T.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            Assert.IsTrue(y.CompareWith(right, 0));

        }

        [TestMethod]
        public void Test_f_SolveUT_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1.65, 8.25, 25.3, 45.65 });
            IVector y;

            y = mar.T.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_SolveUT_ne_plot_nonsimmetr()
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


            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1.5, 3.6, 9, 4.4 });

            IVector result = mar.T.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_SolveUT_ne_plot_simmetr()
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

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 15, 23 });

            IVector result = mar.T.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            Assert.IsTrue(result.CompareWith(right, 0));
        }


        //////////////////MultLT#9

        [TestMethod]
        public void Test_f_MultLT_plot_format_nonsimmetr()
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

            y = mar.T.MultL(x);
            IVector right = new SimpleVector(new double[4] { 10, 9, 7, 4 });
            Assert.IsTrue(y.CompareWith(right, 0));

        }

        [TestMethod]
        public void Test_f_MultLT_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });
            IVector y;

            y = mar.T.MultL(x);
            IVector right = new SimpleVector(new double[4] { 36.3, 46.75, 22.55, 19.8 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_MultLT_ne_plot_nonsimmetr()
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


            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = mar.T.MultL(x);
            IVector right = new SimpleVector(new double[4] { 5.9, 14.6, 11.2, 4.4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_MultLT_ne_plot_simmetr()
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

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = mar.T.MultL(x);
            IVector right = new SimpleVector(new double[4] { 6, 19, 12, 4 });

            Assert.IsTrue(result.CompareWith(right, 0));
        }

        [TestMethod]
        public void Test_f_MultLT_Exception()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[3] { 1, 1, 1 });
            IVector y;
            Assert.ThrowsException<DifferentSizeException>(() => { y = mar.T.MultL(x); });

        }

        //////////////////MultUT#10

        [TestMethod]
        public void Test_f_MultUT_plot_format_nonsimmetr()
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

            y = mar.T.MultU(x);
            IVector right = new SimpleVector(new double[4] { 1, 5, 8, 10 });
            Assert.IsTrue(y.CompareWith(right, 0));

        }

        [TestMethod]
        public void Test_f_MultUT_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1.5, 2.5, 7.5, 1, 2.5, 2.5, 2.5, 7.5, 7.5, 2.5, 3.5, 2.5, 1, 7.5, 2.5, 4.5 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1.1, 2.2, 3.3, 4.4 });
            IVector y;

            y = mar.T.MultU(x);
            IVector right = new SimpleVector(new double[4] { 1.65, 8.25, 25.3, 45.65 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_MultUT_ne_plot_nonsimmetr()
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


            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = mar.T.MultU(x);
            IVector right = new SimpleVector(new double[4] { 1.5, 3.6, 9, 4.4 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_MultUT_ne_plot_simmetr()
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

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });

            IVector result = mar.T.MultU(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 15, 23 });

            Assert.IsTrue(result.CompareWith(right, 0));
        }

        [TestMethod]
        public void Test_f_MultUT_Exception()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[3] { 1, 1, 1 });
            IVector y;
            Assert.ThrowsException<DifferentSizeException>(() => { y = mar.T.MultU(x); });

        }

        //////////////////MultD#11

        [TestMethod]
        public void Test_f_MultD_plot_format()
        {
            (int, int)[] coord = new(int, int)[8];
            double[] val = new double[8] { 1.1, 2.2, 3.3, 4.4, 5.5, 6.6, 7.7, 8.8 };

            for (int i = 0; i < 8; i++)
            {
                coord[i] = (i, i);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[8] { 1.5, 2.5, 1.5, 2.5, 1.5, 2.5, 1.5, 2.5 });
            IVector y;

            y = mar.MultD(x);
            IVector right = new SimpleVector(new double[8] { 1.65, 5.5, 4.95, 11, 8.25, 16.5, 11.55, 22 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_MultD_ne_plot_format()
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
            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[8] { 1.5, 1.5, 1.5, 1.5, 1.5, 1.5, 1.5, 1.5 });
            IVector y;

            y = mar.MultD(x);
            IVector right = new SimpleVector(new double[8] { 1.65, 0, 4.95, 6.6, 8.25, 0, 11.55, 13.2 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }


        //////////////////SolveD#12

        [TestMethod]
        public void Test_f_SolveD_plot_format()
        {
            (int, int)[] coord = new(int, int)[8];
            double[] val = new double[8] { 1.1, 2.2, 3.3, 4.4, 5.5, 6.6, 7.7, 8.8 };

            for (int i = 0; i < 8; i++)
            {
                coord[i] = (i, i);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[8] { 1.65, 5.5, 4.95, 11, 8.25, 16.5, 11.55, 22 });
            IVector y;

            y = mar.SolveD(x);
            IVector right = new SimpleVector(new double[8] { 1.5, 2.5, 1.5, 2.5, 1.5, 2.5, 1.5, 2.5 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_SolveD_size()
        {
            (int, int)[] coord = new(int, int)[8];
            double[] val = new double[8] { 1.1, 2.2, 3.3, 4.4, 5.5, 6.6, 7.7, 8.8 };

            for (int i = 0; i < 8; i++)
            {
                coord[i] = (i, i);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[6] { 1.65, 5.5, 4.95, 11, 8.25, 16.5 });
            IVector y;
            Assert.ThrowsException<DifferentSizeException>(() => { y = mar.SolveD(x); });

        }

        [TestMethod]
        public void Test_f_SolveD_ne_plot_format()
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
            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[8] { 1.65, 0, 4.95, 6.6, 8.25, 0, 11.55, 13.2 });
            IVector y;

            y = mar.SolveD(x);
            IVector right = new SimpleVector(new double[8] { 1.5, 1.5, 1.5, 1.5, 1.5, 1.5, 1.5, 1.5 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }


    }
}
