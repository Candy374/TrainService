using System.Diagnostics;

namespace CommonUtilities.PerformanceRelated
{
    public class NicCounter:CounterBase
    {
        private PerformanceCounter _bandwidthCounter = null;
        private PerformanceCounter _receiveCounter = null;
        private PerformanceCounter _sentCounter = null;

        public NicCounter(string machineName, string interfaceName) : base(machineName)
        {
            InterfaceName = interfaceName;
        }

        public string InterfaceName { get; private set; }
        public float Bandwidth
        {
            get
            {
                if (_bandwidthCounter == null)
                {
                    _bandwidthCounter = new PerformanceCounter("Network Interface", "Current Bandwidth", InterfaceName, MachineName);
                    _bandwidthCounter.NextValue();
                }
                return _bandwidthCounter.NextValue();
            }
        }
        public float ReceiveSpeed
        {
            get
            {
                if (_receiveCounter == null)
                {
                    _receiveCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", InterfaceName, MachineName);
                    _receiveCounter.NextValue();
                }
                return _receiveCounter.NextValue();
            }
        }
        public float SentSpeed
        {
            get
            {
                if (_sentCounter == null)
                {
                    _sentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", InterfaceName, MachineName);
                    _sentCounter.NextValue();
                }
                return _sentCounter == null ? -1 : _sentCounter.NextValue();
            }
        }
    }

}
