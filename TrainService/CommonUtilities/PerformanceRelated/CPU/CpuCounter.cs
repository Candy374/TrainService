using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities.PerformanceRelated
{
    public class CpuCounter:CounterBase
    {
        private PerformanceCounter _cpuCounter = null;
        public CpuCounter(string machineName):base(machineName)
        {
        }

        public float GetTotalProcesserTime()
        {
            if (_cpuCounter==null)
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", MachineName);
                _cpuCounter.NextValue();
            }
            return _cpuCounter.NextValue();
        }


    }
}
