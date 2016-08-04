using CommonUtilities.PerformanceRelated;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities
{
    public class PerformanceCounterHelper
    {
        public string MachineName { get; private set; }
        private CpuCounter _cpuCounter = null;
        private RamCounter _ramCounter = null;
        private NicCounter[] _nicCounters = null;
        private DiskCounter _diskCounter = null;
        private static string[] filter = { "MS TCP Loopback interface", "isatap.", "Tunneling" };


        public static PerformanceCounterHelper GetNewPerformanceCounterFrom(string machineName)
        {
            return new PerformanceCounterHelper(machineName);
        }

        public CpuCounter CPU
        {
            get
            {
                if (_cpuCounter == null)
                {
                    _cpuCounter = new CpuCounter(MachineName);
                }

                return _cpuCounter;
            }
        }

        public RamCounter Memory
        {
            get
            {
                if (_ramCounter == null)
                {
                    _ramCounter = new RamCounter(MachineName);
                }

                return _ramCounter;
            }
        }

        public DiskCounter Disk
        {
            get
            {
                if (_diskCounter == null)
                {
                    _diskCounter = new DiskCounter(MachineName);
                }

                return _diskCounter;
            }
        }

        public NicCounter[] NICs
        {
            get
            {
                if (_nicCounters == null)
                {
                    _nicCounters = GetNICCounters(MachineName);
                }

                return _nicCounters;
            }
        }

        //Not allow default constructor
        private PerformanceCounterHelper() { }

        private PerformanceCounterHelper(string machineName)
        {
            MachineName = machineName;
        }


        private static NicCounter[] GetNICCounters(string machineName)
        {
            string[] nics = GetNICInstances(machineName);
            List<NicCounter> nicCounters = new List<NicCounter>();
            foreach (string nicInstance in nics)
            {
                nicCounters.Add(new NicCounter(machineName, nicInstance));
            }
            return nicCounters.ToArray();
        }

        private static string[] GetNICInstances(string machineName)
        {

            List<string> nics = new List<string>();
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface", machineName);
            var nicNames = category.GetInstanceNames();
            if (nicNames != null)
            {
                foreach (string nic in nicNames)
                {
                    if (filter == null || !filter.Contains(nic))
                    {
                        nics.Add(nic);
                    }
                }
            }

            return nics.ToArray();
        }
    }




}
