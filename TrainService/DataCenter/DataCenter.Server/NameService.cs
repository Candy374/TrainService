using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using DsDomainProxy;

namespace DataCenter.Server
{
    class NameService
    {
        public static NameService Instance { get; private set; }

        static NameService()
        {
            Instance = new NameService();
        }

        private object sync = new object();
        private Dictionary<string, string> userNames = new Dictionary<string, string>();

        private NameService()
        {
        }

        public static string GetCoreId(string input)
        {
            try
            {
                string coreId = input.ToUpper();

                if (coreId.Contains("-"))
                {
                    coreId = coreId.Split('-')[1];
                }

                if (coreId.StartsWith(@"DS\"))
                {
                    coreId = coreId.Substring(3);
                }

                if (coreId.Length == 6)
                {
                    return coreId;
                }
            }
            catch { }

            return input;
        }

        public string GetUserName(string input)
        {
            try
            {
                string coreId = GetCoreId(input);

                if (coreId.Length == 6)
                {
                    if (!this.userNames.ContainsKey(coreId))
                    {
                        lock (sync)
                        {
                            if (!this.userNames.ContainsKey(coreId))
                            {
                                var name = GetUserNameFromProxy(coreId);
                                this.userNames.Add(coreId, name);
                            }
                        }
                    }

                    return this.userNames[coreId];
                }
            }
            catch { }

            return input;
        }

        private static string GetUserNameFromProxy(string coreId)
        {
            try
            {
                using (var factory = new ChannelFactory<IDomainProxy>("DomainProxy"))
                {
                    var proxy = factory.CreateChannel();
                    var userName = proxy.GetUserFullName(coreId);

                    return userName;
                }
            }
            catch { }

            return string.Empty;
        }
    }
}
