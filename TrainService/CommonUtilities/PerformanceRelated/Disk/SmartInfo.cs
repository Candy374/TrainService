using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities.PerformanceRelated
{
    public class SmartInfo
    {
        public bool HasData
        {
            get
            {
                if (Current == 0 && Worst == 0 && Threshold == 0 && Data == 0)
                    return false;
                return true;
            }
        }
        public string Attribute { get; set; }
        public int Current { get; set; }
        public int Worst { get; set; }
        public int Threshold { get; set; }
        public int Data { get; set; }
        public bool IsOK { get; set; }

        public SmartInfo()
        {

        }

        public SmartInfo(string attributeName)
        {
            this.Attribute = attributeName;
        }
    }
}
