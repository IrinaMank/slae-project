using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using slae_project.Matrix;
using slae_project.Vector;
using slae_project.Solver;
using slae_project.Preconditioner;
using slae_project.Logger;

namespace slae_project
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //CoordinateMatrix.localtest();
            //(int, int)[] coord = new(int, int)[100];
            ////   double[] valMatrix = new double[25] { 1, 5, 1, 2, 1, 8, 2, 1, 3, 2, 2, 9, 3, 7, 3, 1, 3, 10, 4, 6, 3, 1, 2, 11, 5 };	
            //// double[] valB = new double[] { 27, 37, 72, 83, 80 };	
            //double[] valX = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //double[] valMatrix = new double[100] { 7, 2, 0, 0, 0, 1, 3, 0, 0, 0, 1, 8, 4, 0, 0, 0, 1, 2, 0, 0, 0, 3, 14, 3, 0, 0, 0, 4, 4, 0, 0, 0, 2, 9, 1, 0, 0, 0, 2, 4, 0, 0, 0, 4, 6, 1, 0, 0, 0, 1, 2, 0, 0, 0, 1, 5, 2, 0, 0, 0, 2, 3, 0, 0, 0, 3, 11, 3, 0, 0, 0, 4, 1, 0, 0, 0, 3, 12, 4, 0, 0, 0, 1, 4, 0, 0, 0, 2, 8, 1, 0, 0, 0, 2, 1, 0, 0, 0, 1, 4 };
            //double[] valB = new double[] { 38.0000, 52.0000, 128.0000, 105.0000, 62.0000, 51.0000, 127.0000, 164.0000, 117.0000, 62.0000 };


            //for (int i = 0; i < 100; i++)
            //{
            //    coord[i] = (i / 10, i % 10);
            //}

            //IMatrix mar = new CoordinateMatrix(coord, valMatrix);
            ////NoPreconditioner preco = new NoPreconditioner();
            //LUPreconditioner preco = new LUPreconditioner(mar);
            ////DiagonalPreconditioner preco = new DiagonalPreconditioner(mar);	

            //IVector b = new SimpleVector(valB);
            //IVector x0 = new SimpleVector(10);
            //IVector rigthX = new SimpleVector(valX);

            //LOSSolver s = new LOSSolver();
            //FileLogger logger = new FileLogger();
            //IVector x = s.Solve(preco,mar, b, x0, 1e-10, 10000, logger);

           // logger.Dispose();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SharpGL_limbo.SharpGL_Open_Test();

            Application.Run(new Form1());
        }
    }
}
