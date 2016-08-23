using LoggerContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtilities;

namespace SimpleLogger
{
    public class Logger : ILogger, IDisposable
    {
        private readonly StreamWriter _writer;
        private const string timeFormat = "HH:mm:ss";
        public Logger()
        {
            _writer = new StreamWriter("SimpleLog" + DateTime.Now.ToString("yyyy_MM_dd") + ".log", true);
            _writer.AutoFlush = true;

        }

        public void Critical(string msg, string title = null)
        {
            _writer.WriteLine(string.Format("[C]\t[{0}]\t[{1}] - {2}", DateTime.Now.ToString(timeFormat), title ?? "title", msg));
        }

        public void Critical(Exception ex, string msg, string title = null)
        {
            _writer.WriteLine(string.Format("[C]\t[{0}]\t[{1}] - {2}\r\n{3}", DateTime.Now.ToString(timeFormat), title ?? "title", msg, ex.ToString("PMTI", "\t\t\t")));
        }

        public void Dispose()
        {
            if (_writer != null)
            {
                try
                {
                    _writer.Flush();
                    _writer.Dispose();
                }
                catch (Exception)
                {
                }
            }
        }

        public void Error(string msg, string title = null)
        {
            _writer.WriteLine("[E]\t[{0}]\t[{1}] - {2}".FormatedWith(DateTime.Now.ToString(timeFormat), title ?? "title", msg));
        }

        public void Error(Exception ex, string msg, string title = null)
        {
            _writer.WriteLine(string.Format("[E]\t[{0}]\t[{1}] - {2}\r\n{3}", DateTime.Now.ToString(timeFormat), title ?? "title", msg, ex.ToString("PMTI", "\t\t\t")));

        }

        public void Info(string msg, string title = null)
        {
            _writer.WriteLine("[I]\t[{0}]\t[{1}] - {2}".FormatedWith(DateTime.Now.ToString(timeFormat), title ?? "title", msg));
        }

        public void Warn(string msg, string title = null)
        {
            _writer.WriteLine("[W]\t[{0}]\t[{1}] - {2}".FormatedWith(DateTime.Now.ToString(timeFormat), title ?? "title", msg));
        }

        public void Warn(Exception ex, string msg, string title = null)
        {
            _writer.WriteLine(string.Format("[W]\t[{0}]\t[{1}] - {2}\r\n{3}", DateTime.Now.ToString(timeFormat), title ?? "title", msg, ex.ToString("PMTI", "\t\t\t")));
        }
    }
}
