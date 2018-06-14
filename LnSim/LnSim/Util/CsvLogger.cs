using System;
using System.IO;

namespace LnSim.Util
{
    public class CsvLogger : IDisposable
    {
        private readonly StreamWriter _writer;
        private const string DataPath = "../../../../../Data/Logs/";
        public CsvLogger(string fileName, bool append = false, string header = null)
        {
            _writer = new StreamWriter($"{DataPath}{fileName}.csv", append);
            _writer.AutoFlush = true;
            if(header != null)
                _writer.WriteLine(header);
        }

        public void WriteLine(string str)
        {
            _writer.WriteLine(str);
            Console.WriteLine(str);
        }

        public void Dispose()
        {
            _writer?.Dispose();
        }
    }
}
