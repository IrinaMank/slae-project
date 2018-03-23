using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            loadWindow window = new loadWindow();
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(3);
            window.Show();
            while (end > DateTime.Now)
                Application.DoEvents();
            window.Close();
            window.Dispose();
            Application.Run(new Form1());
        }
    }
}
