using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerContract
{
    public interface ILogger
    {
        void Info(string msg, string title = null);

        void Error(string msg, string title = null);

        void Error(Exception ex, string msg, string title = null);

        void Warn(Exception ex, string msg, string title = null);

        void Warn(string msg, string title = null);

        void Critical(string msg, string title = null);

        void Critical(Exception ex, string msg, string title = null);
    }
}
