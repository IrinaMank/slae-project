using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using slae_project.Preconditioner;
using slae_project.Vector;
using System.Windows.Forms;
using System.Drawing;

namespace slae_project.Logger
{
    /// <summary>
    /// Своя консоль с блекджеком и... пожалуй, это все, что у нее есть
    /// </summary>
    public class ConWindow
    {
        Form form;
        TextBox tb;
        public bool closed;
        public ConWindow()
        {
            form = new Form();
            form.Show();
            form.Size = new Size(600, 500);
            
            tb = new TextBox();
            tb.Multiline = true;
            tb.Location = new Point(-1, -1);
            tb.Size = new Size(form.Size.Width - 13, form.Size.Height);
            tb.ReadOnly = true;
            tb.BackColor = Color.Black;
            tb.ForeColor = Color.White;
            tb.ScrollBars = ScrollBars.Vertical;
            form.SizeChanged += new EventHandler(delegate (Object sender, EventArgs e) { tb.Size = new Size(form.Size.Width-13, form.Size.Height); });
            form.FormClosing += new FormClosingEventHandler(delegate (object sender, FormClosingEventArgs e) { this.closed = true; });

            tb.Font = new Font("Consolas", 12, FontStyle.Regular);
            form.Controls.Add(tb);
            this.closed = false;
        }

        public void WriteLine(string line)
        {
            tb.Text += line + "\r\n";
        }

        public void Write(string line)
        {
            tb.Text += line;
        }

        public void Show()
        {
            form.Show();
        }
    }
    public class ConsoleLogger : ILogger
    {
        public int maxiter = 0;
        public double per = 0;
        static ConWindow conwindow = new ConWindow();
        public ConsoleLogger()
        {
            if (conwindow.closed)
                conwindow = new ConWindow();
        }
        public void WriteIteration(int number, double residual)
        {
            if (number == 0)
                conwindow.WriteLine("-----------Итерации------------Невязка-------------");
            String msg = String.Format("{0}\t{1}", number, residual);
            conwindow.WriteLine(msg);
#if !TEST
            Form1.updateProgressBar(number);
#endif
            if (number == maxiter - 1)
            {
                MessageBox.Show("Процесс поиска решения остановлен по достижению максимального числа итераций\n Итоговая невязка:" + residual.ToString());
            }
        }

        public void WriteSolution(IVector x, int Maxiter, double residual)
        {
           conwindow.WriteLine("----------------Конечная невзяка-------------------");
           conwindow.WriteLine(residual.ToString());
           conwindow.WriteLine("----------------------Решение----------------------");
#if !TEST
            Form1.updateProgressBar(Maxiter);
#endif
            for (int i = 0; i < x.Size; i++)
            {
                String msg = String.Format("{0}", x[i]);
                conwindow.WriteLine(msg);
            }          
        }

        public void setMaxIter(int i)
        {
            maxiter = i;
        }

        public void WriteNameSolution(string nameSolver, string namePred)
        {
          conwindow.WriteLine("---------------------------------------------------");
          conwindow.WriteLine("Решатель: " + nameSolver);
          conwindow.WriteLine("Предобуславливание: " + namePred);
        }

        public void WriteTime(string start, string end)
        {
            conwindow.WriteLine("---------------------------------------------------");
            conwindow.WriteLine("Время начала решения:\t\t" + start);
            conwindow.WriteLine("Время окончания решения:\t" + end);
            conwindow.WriteLine("---------------------------------------------------");
            conwindow.WriteLine("\n\n\n");
        }

        public void Dispose()
        {

        }
    }
}
