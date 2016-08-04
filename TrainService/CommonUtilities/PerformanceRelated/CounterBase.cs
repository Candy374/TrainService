using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities.PerformanceRelated
{
    public abstract class CounterBase
    {
        public string MachineName
        {
            get; private set;
        }

        public CounterBase(string machineName)
        {
            MachineName = machineName;
        }
    }
}
