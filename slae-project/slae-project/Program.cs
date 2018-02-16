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

            (int, int)[] coord = new(int, int)[9];
            double[] val = new double[9] { 1,3,3,1,1,2,1,1,1};

            for (int i = 0; i < 9; i++)
            {
                coord[i] = (i/3, i%3);
            }

            IMatrix mar = new CoordinateMatrix(coord,val);
            IVector x = new SimpleVector(new double[3] { 1, 2, 3 });

            IVector y = mar.Mult(x);
            y = mar.SolveL(x);
            y = mar.SolveU(y);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
           
        }
    }
}
