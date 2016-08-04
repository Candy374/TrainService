using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace CommonUtilities.PerformanceRelated
{
    public class VolumeCounter:CounterBase
    {
        private PerformanceCounter _freeSpaceCounter = null;
        private PerformanceCounter _freeSpacePercentageCounter = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="volumeName">VolumeChar and ':', such as: "C:"</param>
        public VolumeCounter(string machineName, string volumeName):base(machineName)
        {
            VolumeName = volumeName;
        }

        public readonly string VolumeName;

        /// <summary>
        /// Get free space in MB
        /// </summary>
        public float FreeSpace
        {
            get
            {
                if (_freeSpaceCounter == null)
                {
                    _freeSpaceCounter = new PerformanceCounter("LogicalDisk", "Free Megabytes", VolumeName, MachineName);
                    _freeSpaceCounter.NextValue();
                }

                return _freeSpaceCounter.NextValue();
            }
        }

        public float FreeSpacePercentage
        {
            get
            {
                if (_freeSpacePercentageCounter == null)
                {
                    _freeSpacePercentageCounter = new PerformanceCounter("LogicalDisk", "% Free Space", VolumeName, MachineName);
                    _freeSpacePercentageCounter.NextValue();
                }

                return _freeSpacePercentageCounter.NextValue();
            }

        }
      
    }
}
