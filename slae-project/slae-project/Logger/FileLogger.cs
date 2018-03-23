using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using slae_project.Vector;

namespace slae_project.Logger
{
    
    public class FileLogger : ILogger, IDisposable
    {
        public int iter = 0;
        public double per = 0;
        private System.IO.StreamWriter fileStream;
        private string defaultLogFile = "./log.txt";

        public FileLogger()
        {
            setFile(defaultLogFile);
        }
        public void Dispose()
        {
            fileStream.Dispose();
        }
        public void setFile(string filename)
        {
            var dirName = filename.Substring(0, filename.LastIndexOf('/'));
            if (!System.IO.Directory.Exists(dirName))
                System.IO.Directory.CreateDirectory(dirName);
            fileStream = new System.IO.StreamWriter(filename);
        }
        public void WriteIteration(int number, double residual, double percent)
        {
            iter = number;
            per = percent;
            String msg = String.Format("{0}\t{1}", number, residual);
            fileStream.WriteLine(msg);
            fileStream.Flush();
            //Thread.Sleep(0);
        }

        public void WriteSolution(IVector sol)
        {
            for (int i = 0; i < sol.Size; i++)
            {
                String msg = String.Format("{0}", sol[i]);
                fileStream.WriteLine(msg);

            }
            fileStream.Flush();
        }
    }
}
