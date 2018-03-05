using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using slae_project.Matrix;
using slae_project.Vector.VectorExceptions;
using slae_project.Vector;
using slae_project.Solver;
namespace UnitTestProject
{
    [TestClass]
    public class LOSTests
    {
        //вычислительный тест
        //Результат: не пройдено
        [TestMethod]
        public void calculateTest()
        {
            (int, int)[] coord = new(int, int)[25];
            double[] valMatrix = new double[25] { 1, 5, 1, 2, 1, 8, 2, 1, 3, 2, 2, 9, 3, 7, 3, 1, 3, 10, 4, 6, 3, 1, 2, 11, 5};
            double[] valB = new double[] { 27, 37, 72, 83, 80 };
            double[] valX = new double[] { 1, 2, 3, 4, 5 };

            for (int i = 0; i < 25; i++)
            {
                coord[i] = (i / 5, i % 5);
            }

            IMatrix mar = new CoordinateMatrix(coord, valMatrix);

            IVector b = new SimpleVector(valB);
            IVector x0 = new SimpleVector(5);
            IVector rigthX = new SimpleVector(valX);

            LOSSolver s = new LOSSolver();
            IVector x = s.Solve(mar, b, x0, 1e-8, 10000);

            Assert.IsTrue(x.CompareWith(rigthX, 1e-5));
        }

        //тест с нулевой правой частью
        //Результат:пройдено
        [TestMethod]
        public void bNullTest()
        {
            (int, int)[] coord = new(int, int)[25];
            double[] valMatrix = new double[25] { 1, 5, 1, 2, 1, 8, 2, 1, 3, 2, 2, 9, 3, 7, 3, 1, 3, 10, 4, 6, 3, 1, 2, 11, 5 };
            double[] valB = new double[] { 0, 0, 0, 0, 0};
            double[] valX = new double[] { 0, 0, 0, 0, 0};

            for (int i = 0; i < 25; i++)
            {
                coord[i] = (i / 5, i % 5);
            }

            IMatrix mar = new CoordinateMatrix(coord, valMatrix);

            IVector b = new SimpleVector(valB);
            IVector x0 = new SimpleVector(5);
            IVector rigthX = new SimpleVector(valX);

            LOSSolver s = new LOSSolver();
            IVector x = s.Solve(mar, b, x0, 1e-8, 10000);

            Assert.IsTrue(x.CompareWith(rigthX, 1e-8));
        }

        //тест с диагональной матрицей
        //Результат: проходит с точностью 1е-4
        [TestMethod]
        public void diagonalMatrix()
        {
            (int, int)[] coord = new(int, int)[3];
            double[] valMatrix = new double[3] { 1, 2, 3 };
            double[] valB = new double[] { 1, 4, 9 };
            double[] valX = new double[] { 1, 2, 3 };

            coord[0] = (0, 0);
            coord[1] = (1, 1);
            coord[2] = (2, 2);

            IMatrix mar = new CoordinateMatrix(coord, valMatrix);

            IVector b = new SimpleVector(valB);
            IVector x0 = new SimpleVector(3);
            IVector rigthX = new SimpleVector(valX);

            LOSSolver s = new LOSSolver();
            IVector x = s.Solve(mar, b, x0, 1e-8, 10000);

            Assert.IsTrue(x.CompareWith(rigthX, 1e-4));
        }
    }
}
