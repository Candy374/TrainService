using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Javis.Mark42;

namespace DataCenter.Eyes
{
    class Program
    {
        static ManualResetEvent exit = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            var host = new ServiceHost("Service.xml");
            host.Start();

            System.Console.WriteLine("Host is ready. {0}Type \"exit<Enter>\" to stop and exit. {0}", Environment.NewLine);

            while (!exit.WaitOne(0))
            {
                if (System.Console.ReadLine().Equals("exit", StringComparison.InvariantCultureIgnoreCase))
                {
                    exit.Set();
                    Console.WriteLine("Will close service... (Need wait a while) ");
                }
                else
                {
                    PrintStates(host);
                }
            }

            host.Stop();
        }

        private static void PrintStates(ServiceHost host)
        {
            try
            {
                var states = host.GetStates();
                foreach (var p in states)
                {
                    Console.WriteLine("\t{0}:\t{1}", p.Key, p.Value.ToString());
                }
            }
            catch { }
        }
    }
}
