using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities.PerformanceRelated
{
    public class RamCounter : CounterBase
    {
        private PerformanceCounter _ramCounter;

        public RamCounter(string machineName) : base(machineName)
        {

        }

        public float GetAvailableMemoryInMBytes()
        {
            if (_ramCounter == null)
            {
                _ramCounter = new PerformanceCounter("Memory", "Available MBytes", String.Empty, MachineName);
                _ramCounter.NextValue();
            }

            return _ramCounter.NextValue();
        }
    }
}
