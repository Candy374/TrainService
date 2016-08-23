using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtilities;
using System.IO;

namespace LoggerContract
{
    public static class Logger
    {
        private static Dictionary<string, ILogger> _loggers = new Dictionary<string, LoggerContract.ILogger>();

        static Logger()
        {
            var models = AppConfigHelper.LoadAppSetting("LoggerModelPath", "SimpleLogger.dll").Split('|');
            foreach (var model in models)
            {
                var path = GetAssemblyPath(model);
                if (path == null)
                {
                    continue;
                }
                var types = ReflectionHelper.LoadAssembly(path).GetTypes();
                foreach (var t in types)
                {
                    if (!_loggers.ContainsKey(t.FullName))
                    {
                        if (t.GetInterface("ILogger") != null)
                        {
                            _loggers.Add(t.FullName, ReflectionHelper.CreateInstance<ILogger>(t));
                            break;
                        }
                    }
                }
            }
        }

        private static string GetAssemblyPath(string model)
        {
            if (File.Exists(model))
            {
                return model;
            }

            var path = Path.Combine(Path.GetDirectoryName(typeof(Logger).Assembly.CodeBase), model);
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        public static void Critical(string msg, string title = null)
        {
            foreach (var item in _loggers.Values)
            {
                try
                {
                    item.Critical(msg, title);
                }
                catch (Exception) { }
            }
        }

        public static void Critical(Exception ex, string msg, string title = null)
        {
            foreach (var item in _loggers.Values)
            {
                try
                {
                    item.Critical(ex, msg, title);
                }
                catch (Exception) { }
            }
        }

        public static void Error(string msg, string title = null)
        {
            foreach (var item in _loggers.Values)
            {
                try
                {
                    item.Error(msg, title);
                }
                catch (Exception) { }
            }
        }

        public static void Error(Exception ex, string msg, string title = null)
        {
            foreach (var item in _loggers.Values)
            {
                try
                {
                    item.Error(ex, msg, title);
                }
                catch (Exception) { }
            }
        }

        public static void Info(string msg, string title = null)
        {
            foreach (var item in _loggers.Values)
            {
                try
                {
                    item.Info(msg, title);
                }
                catch (Exception) { }
            }
        }

        public static void Warn(string msg, string title = null)
        {
            foreach (var item in _loggers.Values)
            {
                try
                {
                    item.Warn(msg, title);
                }
                catch (Exception) { }
            }
        }

        public static void Warn(Exception ex, string msg, string title = null)
        {
            foreach (var item in _loggers.Values)
            {
                try
                {
                    item.Warn(ex, msg, title);
                }
                catch (Exception) { }
            }
        }
    }
}
