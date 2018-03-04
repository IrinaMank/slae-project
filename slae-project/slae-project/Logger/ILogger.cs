using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.ILogger
{
    public class Logger:IDisposable
    {
        private System.IO.StreamWriter fileStream;
        private string defaultLogFile = "../log/log.txt";

        public Logger()
        {
            setFile(defaultLogFile);
        }
        public void setFile(string filename)
        {
            var dirName = filename.Substring(0, filename.LastIndexOf('/'));
            if (!System.IO.Directory.Exists(dirName))
                System.IO.Directory.CreateDirectory(dirName);
            fileStream = new System.IO.StreamWriter(filename);

        }
        public void writeIteration(int number, double residual)
        {
            String msg =  String.Format("Iteration: {0} \t residual: {1}", number, residual);
            fileStream.WriteLine(msg);
            fileStream.Flush();
        }

        public void Dispose()
        {
            fileStream.Dispose();
        }
    }
}
