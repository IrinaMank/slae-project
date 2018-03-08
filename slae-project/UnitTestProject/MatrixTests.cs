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

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_Mult_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 2, 5, 1, 2, 2, 2, 5, 5, 2, 3, 2, 1, 5, 2, 4 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });
            IVector y;

            y = mar.Mult(x);
            IVector right = new SimpleVector(new double[4] { 24, 32, 26, 33 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        //неплотная несимметричная матрица
        [TestMethod]
        public void Test_f_Mult_ne_plot_format_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[14];
            double[] val = new double[14] { 1, 2, 3, 4, 5, 6, 7, 8, 23, 52, 6, 9, 34, 5 };

            for (int i = 0; i < 8; i++)
            {
                coord[i] = (i / 4, i % 4);
            }
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

            Assert.IsTrue(result.CompareWith(right, 1e-5));
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
            Assert.IsTrue(y.CompareWith(right, 1e-5));

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
            Assert.IsTrue(y.CompareWith(right, 1e-5));
            //13, 0.5, 0.5, -4
            //5, 0, 0, -1
        }

        [TestMethod]
        public void Test_f_SolveL_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 2, 5, 1, 2, 2, 2, 5, 5, 2, 3, 2, 1, 5, 2, 4 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });
            IVector y;

            y = mar.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 0, -2, 0 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_SolveL_ne_plot_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[14];
            double[] val = new double[14] { 1, 2, 3, 4, 5, 6, 7, 8, 23, 52, 6, 9, 34, 5 };

            for (int i = 0; i < 8; i++)
            {
                coord[i] = (i / 4, i % 4);
            }
            coord[8] = (2, 0);
            coord[9] = (2, 2);
            coord[10] = (2, 3);
            coord[11] = (3, 0);
            coord[12] = (3, 1);
            coord[13] = (3, 3);

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = mar.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, -3, 29 / 2.0, -839 / 150.0 });

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
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = mar.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, -9, -30 / 31.0 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_SolveL_degenerate_matrix()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16];

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
                val[i] = 1;
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 2, 2 });
            IVector y;

            y = mar.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));
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
                x[i] = 100;
                right[i] = 1;
            }

            IVector y;
            y = mar.SolveL(x);
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }


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
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });
            IVector y;

            y = mar.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 13, 0.5, 0.5, -4 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_SolveU_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 2, 5, 1, 2, 2, 2, 5, 5, 2, 3, 2, 1, 5, 2, 4 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 9, -7, -5, -15 });
            IVector y;

            y = mar.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }


        [TestMethod]
        public void Test_f_SolveU_ne_plot_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[14];
            double[] val = new double[14] { 1, 2, 3, 4, 5, 6, 7, 8, 23, 52, 6, 9, 34, 5 };

            for (int i = 0; i < 8; i++)
            {
                coord[i] = (i / 4, i % 4);
            }
            coord[8] = (2, 0);
            coord[9] = (2, 2);
            coord[10] = (2, 3);
            coord[11] = (3, 0);
            coord[12] = (3, 1);
            coord[13] = (3, 3);

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 306, -912, 3975, -2857 });

            IVector result = mar.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 75 });

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
            IVector x = new SimpleVector(new double[4] { 6, 939, -4929 / 2.0, -2926 });

            IVector result = mar.SolveU(x);
            IVector right = new SimpleVector(new double[4] { 1, 1, 1, 93 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
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
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_MultL_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 2, 5, 1, 2, 2, 2, 5, 5, 2, 3, 2, 1, 5, 2, 4 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            IVector y;

            y = mar.MultL(x);
            IVector right = new SimpleVector(new double[4] { 1, 3, 10, -1 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_MultL_ne_plot_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[14];
            double[] val = new double[14] { 1, 2, 3, 4, 5, 6, 7, 8, 23, 52, 6, 9, 34, 5 };

            for (int i = 0; i < 8; i++)
            {
                coord[i] = (i / 4, i % 4);
            }
            coord[8] = (2, 0);
            coord[9] = (2, 2);
            coord[10] = (2, 3);
            coord[11] = (3, 0);
            coord[12] = (3, 1);
            coord[13] = (3, 3);

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = mar.MultL(x);
            IVector right = new SimpleVector(new double[4] { 1, 7, 49, 66 / 25.0 });

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
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = mar.MultL(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 15, 486 / 31.0 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_MultL_degenerate_matrix()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16];

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
                val[i] = 1;
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            IVector y;

            y = mar.MultL(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 2, 2 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));
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
            IVector x = new SimpleVector(new double[4] { 13, 0.5, 0.5, -4 });
            IVector y;

            y = mar.MultU(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, 3, 4 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_MultU_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 2, 5, 1, 2, 2, 2, 5, 5, 2, 3, 2, 1, 5, 2, 4 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            IVector y;

            y = mar.MultU(x);
            IVector right = new SimpleVector(new double[4] { 9, -7, -5, -15 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }


        [TestMethod]
        public void Test_f_MultU_ne_plot_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[14];
            double[] val = new double[14] { 1, 2, 3, 4, 5, 6, 7, 8, 23, 52, 6, 9, 34, 5 };

            for (int i = 0; i < 8; i++)
            {
                coord[i] = (i / 4, i % 4);
            }
            coord[8] = (2, 0);
            coord[9] = (2, 2);
            coord[10] = (2, 3);
            coord[11] = (3, 0);
            coord[12] = (3, 1);
            coord[13] = (3, 3);

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 75 });

            IVector result = mar.MultU(x);
            IVector right = new SimpleVector(new double[4] { 306, -912, 3975, -2857 });

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
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 93 });

            IVector result = mar.MultU(x);
            IVector right = new SimpleVector(new double[4] { 6, 939, -4929 / 2.0, -2926 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
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

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_MultT_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 2, 5, 1, 2, 2, 2, 5, 5, 2, 3, 2, 1, 5, 2, 4 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });
            IVector y;

            y = mar.T.Mult(x);
            IVector right = new SimpleVector(new double[4] { 24, 32, 26, 33 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        //неплотная несимметричная матрица
        [TestMethod]
        public void Test_f_MultT_ne_plot_format_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[14];
            double[] val = new double[14] { 1, 5, 23, 9, 2, 6, 34, 3, 7, 52, 4, 8, 6, 5 };

            for (int i = 0; i < 6; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            for (int i = 6; i < 10; i++)
            {
                coord[i] = ((i + 1) / 4, (i + 1) % 4);
            }

            for (int i = 10; i < 14; i++)
            {
                coord[i] = ((i + 2) / 4, (i + 2) % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = mar.T.Mult(x);
            IVector right = new SimpleVector(new double[4] { 30, 70, 203, 97 });

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

            Assert.IsTrue(result.CompareWith(right, 1e-5));
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
            Assert.IsTrue(y.CompareWith(right, 1e-5));

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
            double[] val = new double[16] { 1, 1, 1, 1, 4, 1, 1, 1, 4, 3, 1, 1, 4, 3, 2, 1 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            IVector y;

            y = mar.T.MultL(x);
            IVector right = new SimpleVector(new double[4] { 1, 5, 16 / 3.0, 35 / 6.0 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));
        }

        [TestMethod]
        public void Test_f_SolveLT_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 2, 5, 1, 2, 2, 2, 5, 5, 2, 3, 2, 1, 5, 2, 4 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });
            IVector y;

            y = mar.T.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 0, -2, 0 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_SolveLT_ne_plot_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[14];
            double[] val = new double[14] { 1, 5, 23, 9, 2, 6, 34, 3, 7, 52, 4, 8, 6, 5 };

            for (int i = 0; i < 6; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            for (int i = 6; i < 10; i++)
            {
                coord[i] = ((i + 1) / 4, (i + 1) % 4);
            }

            for (int i = 10; i < 14; i++)
            {
                coord[i] = ((i + 2) / 4, (i + 2) % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = mar.T.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, -3, 29 / 2.0, -839 / 150.0 });

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
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });

            IVector result = mar.T.SolveL(x);
            IVector right = new SimpleVector(new double[4] { 1, 2, -9, -30 / 31.0 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
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
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_MultLT_plot_format_simmetr()
        {
            (int, int)[] coord = new(int, int)[16];
            double[] val = new double[16] { 1, 2, 5, 1, 2, 2, 2, 5, 5, 2, 3, 2, 1, 5, 2, 4 };

            for (int i = 0; i < 16; i++)
            {
                coord[i] = (i / 4, i % 4);
            }

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 2, 3, 4 });
            IVector y;

            y = mar.T.MultL(x);
            IVector right = new SimpleVector(new double[4] { 24, 8, -3, 4 });
            Assert.IsTrue(y.CompareWith(right, 1e-5));

        }

        [TestMethod]
        public void Test_f_MultLT_ne_plot_nonsimmetr()
        {
            (int, int)[] coord = new(int, int)[14];
            double[] val = new double[14] { 1, 2, 3, 4, 5, 6, 7, 8, 23, 52, 6, 9, 34, 5 };

            for (int i = 0; i < 8; i++)
            {
                coord[i] = (i / 4, i % 4);
            }
            coord[8] = (2, 0);
            coord[9] = (2, 2);
            coord[10] = (2, 3);
            coord[11] = (3, 0);
            coord[12] = (3, 1);
            coord[13] = (3, 3);

            IMatrix mar = new CoordinateMatrix(coord, val);
            IVector x = new SimpleVector(new double[4] { 1, 1, 2, 75 });

            IVector result = mar.T.MultL(x);
            IVector right = new SimpleVector(new double[4] { 727, -276, -57, 75 });

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
            IVector x = new SimpleVector(new double[4] { 1, 1, 2, 93 });

            IVector result = mar.T.MultL(x);
            IVector right = new SimpleVector(new double[4] { 11, 473, 54, 93 });

            Assert.IsTrue(result.CompareWith(right, 1e-5));
        }



        //////////////////SolveL + SolveU

        [TestMethod]
        public void Test_f_SolveL_SolveU_plot_format_nonsimmetr()
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
