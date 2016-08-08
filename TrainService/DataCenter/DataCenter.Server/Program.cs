using System;
using System.Linq;
using Microsoft.Owin.Hosting;
using System.Net.Http;

namespace DataCenter.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            DataService.Instance.Start();

            string[] hostnames = new string[] { "http://localhost:3001/", "http://nebula:3001/" };
            var option = new StartOptions();
            foreach(var name in hostnames)
            {
                option.Urls.Add(name);
            }
            var web = WebApp.Start<Startup>(option);

            System.Console.WriteLine("Service is ready. {0}Web API is hosted at {1}. {0}Type \"exit<Enter>\" to stop and exit. {0}", Environment.NewLine, string.Join(", ", hostnames));

            bool exit = false;
            do
            {
                exit = Console.ReadLine().ToLower().Equals("exit");
            }
            while (!exit);
            Console.WriteLine("Call DataService.Instance.Abort()...");
            DataService.Instance.Abort();
            web.Dispose();
        }
    }
}
