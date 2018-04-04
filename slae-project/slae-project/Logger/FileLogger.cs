//#define TEST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using slae_project.Vector;
using slae_project.Preconditioner;
using System.Windows.Forms;

namespace slae_project.Logger
{
    
    public class FileLogger : ILogger, IDisposable
    {
        public int maxiter = 0;
        public double per = 0;
        private System.IO.StreamWriter fileStream;
        public string filename;

        public FileLogger(string filename = "log.txt")
        {
            setFile(filename);
        }
        public void Dispose()
        {
            fileStream.Dispose();
        }
        public void setFile(string fn)
        {
            filename = fn;
            fileStream = new System.IO.StreamWriter(fn);
            fileStream.Dispose();
        }
        public void WriteIteration(int number, double residual)
        {
           
            if (number == 0)
                fileStream.WriteLine("-----------Итерации------------Невязка-------------");
            String msg = String.Format("{0}\t{1}", number, residual);
            fileStream.WriteLine(msg);
#if !TEST
            Form1.updateProgressBar(number);
#endif
            if (number == maxiter-1)
            {
                MessageBox.Show("Процесс поиска решения остановлен по достижению максимального числа итераций\n Итоговая невязка:"+residual.ToString());
            }
        }

        public void WriteNameSolution(string nameSolver,  string namePred)
        {
            fileStream.WriteLine("---------------------------------------------------");
            fileStream.WriteLine("Решатель: "+nameSolver);
            fileStream.WriteLine("Предобуславливание: " + namePred);
            
        }

        public void WriteTime(string start, string end)
        {
            fileStream.WriteLine("---------------------------------------------------");
            fileStream.WriteLine("Время начала решения:\t\t" + start);
            fileStream.WriteLine("Время окончания решения:\t"+ end );
            fileStream.WriteLine("---------------------------------------------------");
            fileStream.WriteLine("\n\n\n");
            fileStream.Dispose();
        }

        public void WriteSolution(IVector sol, int Maxiter, double residual)
        {
            fileStream.WriteLine("----------------Конечная невзяка-------------------");
            fileStream.WriteLine(residual.ToString());
            fileStream.WriteLine("----------------------Решение----------------------");
#if !TEST
            Form1.updateProgressBar(Maxiter);
#endif
            for (int i = 0; i < sol.Size; i++)
            {
                String msg = String.Format("{0}", sol[i]);
                fileStream.WriteLine(msg);
            }
            fileStream.Flush();
        }

        void ILogger.setMaxIter(int i)
        {
            maxiter = i;
        }

        //Этот кусок я посвящаю своей школьной учительнице по программированию. Катя, я так и не научился писать логичный код, но я очень стараюсь.
        public ILogger returnThis()
        {
                fileStream = new System.IO.StreamWriter(filename,true);
            return this;
        }
        
    }
}
