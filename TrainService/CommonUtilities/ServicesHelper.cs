using System;
using System.Linq;
using System.ServiceProcess;
using System.Threading;


namespace CommonUtilities
{
    public class ServiceHelper
    {
        /// <summary>
        /// Stop service <para>servicename</para> on PC <para>machine</para>. 
        /// Check and termanite the related process <para>processname</para> if it doesn't quit after stop service.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="servicename"></param>
        /// <param name="processname"></param>
        public static void StopService(string machine, string servicename, string processname)
        {
            var services = ServiceController.GetServices(machine);
            var service = services.FirstOrDefault(s => s.ServiceName == servicename);

            if (service != null)
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    Thread.Sleep(2000);
                }

                Console.WriteLine("Terminating process:" + processname);
                ProcessHelper.Kill(processname);
            }
        }

        /// <summary>
        /// Start service <para>servicename</para> on PC <para>machine</para>. 
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="servicename"></param>
        public static void StartService(string machine, string servicename)
        {
            var services = ServiceController.GetServices(machine);
            var service = services.FirstOrDefault(s => s.ServiceName == servicename);
            
            if (service == null)
            {
                return;
            }
            
            if (service.Status != ServiceControllerStatus.Stopped)
            {
                return;
            }
            
            service.Start();
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Change the executable path of service <para>serviceName</para> on PC <para>machine</para> to <para>bin</para>.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="serviceName"></param>
        /// <param name="bin"></param>
        public static void ChangeServiceBinPath(string machine, string serviceName, string bin)
        {
            var services = ServiceController.GetServices(machine);
            var service = services.FirstOrDefault(s => s.ServiceName == serviceName);

            if(service == null)
            {
                return;
            }

            var p = ProcessHelper.ExecuteProgram("sc.exe", string.Format(@"\\{0} config {1} start= demand binPath= ""{2}""", machine, serviceName, bin));
            p.WaitForExit();
            p.Close();
        }
    }
}
