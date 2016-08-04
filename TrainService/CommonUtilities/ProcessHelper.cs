#region Using

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;

#endregion


namespace CommonUtilities
{
    public class ProcessHelper
    {


        [Activity]
        public static Process ExecuteProgram(string exePath,
            string args = "", bool showWindow = false,
            bool suppressDebugDialog = false)
        {
            var proc = new ProcessStartInfo();

            if (showWindow)
            {
                proc.WindowStyle = ProcessWindowStyle.Normal;
            }
            else
            {
                proc.WindowStyle = ProcessWindowStyle.Hidden;
            }
            proc.FileName = exePath;
            proc.WorkingDirectory = Path.GetDirectoryName(exePath) ?? "";
            proc.CreateNoWindow = false;
            proc.UseShellExecute = true;

            proc.Arguments = args;

            if (suppressDebugDialog)
            {
                var oldMode = NativeMethods.SetErrorMode(3);
                var process = Process.Start(proc);
                NativeMethods.SetErrorMode(oldMode);
                return process;
            }

            return Process.Start(proc);
        }

        [Activity]
        public static Process ExecuteProgramAndRedirectToFile(string exePath, string args,
            string fileNameFormate, string characteristicString = "",
            bool suppressDebugDialog = false)
        {
            var cmdName = Path.GetFileNameWithoutExtension(exePath) + characteristicString + Guid.NewGuid() + "_start.cmd";
            using (var writer = new StreamWriter(cmdName))
            {
                writer.WriteLine("\"{0}\" {1} > \"{2}\"", exePath, args, string.Format(fileNameFormate, Path.GetFileNameWithoutExtension(exePath) + (characteristicString ?? "")));
            }

            return ExecuteProgram(cmdName, "", false, suppressDebugDialog);
        }

        [Activity]
        public static Process RunAs(string inUserName, string inPassWord,
            string exepath, string args = "", string workingDir = "",
            bool showWindow = false, bool suppressDebugDialog = false)
        {
            var mbrtProcess = new Process();
            mbrtProcess.StartInfo.UserName = inUserName;
            var strPwd = inPassWord;
            var password = new SecureString();
            foreach (char c in strPwd)
            {
                password.AppendChar(c);
            }
            mbrtProcess.StartInfo.Password = password;
            mbrtProcess.StartInfo.WorkingDirectory = string.IsNullOrEmpty(workingDir) ? AppDomain.CurrentDomain.BaseDirectory : workingDir;
            mbrtProcess.StartInfo.FileName = exepath;
            mbrtProcess.StartInfo.Arguments = args;
            mbrtProcess.StartInfo.UseShellExecute = showWindow;

            if (suppressDebugDialog)
            {
                var oldMode = NativeMethods.SetErrorMode(3);
                mbrtProcess.Start();
                NativeMethods.SetErrorMode(oldMode);
                return mbrtProcess;
            }

            mbrtProcess.Start();

            return mbrtProcess;
        }

        public static Process GetFirstProcessByName(string processname)
        {
            Process[] ps = Process.GetProcessesByName(processname);
            Process result = null;
            if (ps != null)
            {
                foreach (Process p in ps)
                {
                    if (p.ProcessName.Equals(processname, StringComparison.CurrentCultureIgnoreCase))
                    {
                        result = p;
                        break;
                    }
                }
            }
            return result;
        }

        [Activity]
        public static bool Kill(string processName)
        {
            for (int i = 0; i < 10; i++)
            {
                var processes = Process.GetProcessesByName(processName);
                foreach (var process in processes)
                {
                    try
                    {
                        process.Kill();

                    }
                    catch (Exception)
                    {
                    }
                }

                if (Process.GetProcessesByName(processName).Length == 0)
                {
                    return true;
                }

                Thread.Sleep(1000);
            }

            return false;
        }

        [Activity]
        public static void Kill(int processId)
        {

            var processes = Process.GetProcesses();
            var process = processes.FirstOrDefault(p => p.Id == processId);
            if (process == null)
            {
                return;
            }

            try
            {
                process.Kill();
            }
            catch (Exception)
            {
            }

        }

        public static bool CreateMutexIdentity(string identityString)
        {
            bool createdNew;
            Mutex instance = new Mutex(true, identityString, out createdNew);
            if (createdNew)
            {
                instance.ReleaseMutex();
                return true;
            }

            return false;
        }

    }
}

