using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using slae_project.Matrix;
using slae_project.Vector;
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
            //(int, int)[] coord = new(int, int)[16];
            //double[] val = new double[16] { 1, 4, 4, 4, 1, 1, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1 };

            //for (int i = 0; i < 16; i++)
            //{
            //    coord[i] = (i / 4, i % 4);
            //}

            //IMatrix mar = new CoordinateMatrix(coord, val);
            //IVector x = new SimpleVector(new double[4] { 1, 1, 1, 1 });
            //IVector y;

            //y = mar.MultL(x);
            //IVector right = new SimpleVector(new double[4] { 1, 2, 3, 4 });
            //y.CompareWith(right, 1e-5);
            CoordinateMatrix.localtest();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
