using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities.PerformanceRelated
{
    public class DiskCounter : CounterBase
    {
        Dictionary<Volumes, VolumeCounter> _volumeCounters;
        Dictionary<int, DiskSmartInfo> _smartInfo;
        public DiskCounter(string machineName) : base(machineName)
        { }

        public void AddVolumeCounter(Volumes volume)
        {
            int param = (int)volume;
            unchecked
            {
                for (int mask = 1; mask > 0; mask = mask << 1)
                {
                    if ((param & mask) == mask)
                    {
                        try
                        {
                            AddVolume((Volumes)mask);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        public VolumeCounter GetVolumeCounter(Volumes v)
        {
            if (_volumeCounters == null)
            {
                return null;
            }

            if (!_volumeCounters.ContainsKey(v))
            {
                return null;
            }

            return _volumeCounters[v];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">Must be single value</param>
        private void AddVolume(Volumes v)
        {
            if (_volumeCounters == null)
            {
                _volumeCounters = new Dictionary<Volumes, VolumeCounter>();
            }

            if (!_volumeCounters.ContainsKey(v))
            {
                _volumeCounters.Add(v, new VolumeCounter(MachineName, v.ToString() + ":"));
            }
        }

        public DiskSmartInfo[] SmartInfos
        {
            get
            {
                if (_smartInfo == null)
                {
                    _smartInfo = GetAllDiskSmartInfo();
                }

                return _smartInfo.Values.ToArray();
            }
        }

        private Dictionary<int, DiskSmartInfo> GetAllDiskSmartInfo()
        {
            var cimv2 = @"\\" + MachineName + @"\root\cimv2";
            var wmiPath = @"\\" + MachineName + @"\root\wmi";
            // retrieve list of drives on computer (this will return both HDD's and CDROM's and Virtual CDROM's)                    
            var dicDrives = new Dictionary<int, DiskSmartInfo>();

            var wdSearcher = new ManagementObjectSearcher("SELECT * FROM CIM_DiskDrive");
            wdSearcher.Scope = new ManagementScope(cimv2);
            // extract model and interface information
            int iDriveIndex = 0;
            var drivers = wdSearcher.Get();
            foreach (ManagementObject drive in drivers)
            {
                var hdd = new DiskSmartInfo();
                hdd.Model = drive["Model"].ToString().Trim();
                hdd.Type = drive["InterfaceType"].ToString().Trim();
                hdd.Size = (ulong)drive["Size"];
                var sn = drive["SerialNumber"];
                hdd.Serial = sn == null ? "Unknown" : sn.ToString().Trim();
                dicDrives.Add(iDriveIndex, hdd);
                iDriveIndex++;
            }

            // get wmi access to hdd 
            var searcher = new ManagementObjectSearcher("Select * from Win32_DiskDrive");
            searcher.Scope = new ManagementScope(wmiPath);

            // check if SMART reports the drive is failing
            searcher.Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictStatus");
            iDriveIndex = 0;
            foreach (ManagementObject drive in searcher.Get())
            {
                dicDrives[iDriveIndex].IsOK = (bool)drive.Properties["PredictFailure"].Value == false;
                iDriveIndex++;
            }

            // retrive attribute flags, value worste and vendor data information
            searcher.Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictData");
            iDriveIndex = 0;
            foreach (ManagementObject data in searcher.Get())
            {
                Byte[] bytes = (Byte[])data.Properties["VendorSpecific"].Value;
                for (int i = 0; i < 30; ++i)
                {
                    try
                    {
                        int id = bytes[i * 12 + 2];

                        int flags = bytes[i * 12 + 4]; // least significant status byte, +3 most significant byte, but not used so ignored.
                                                       //bool advisory = (flags & 0x1) == 0x0;
                        bool failureImminent = (flags & 0x1) == 0x1;
                        //bool onlineDataCollection = (flags & 0x2) == 0x2;

                        int value = bytes[i * 12 + 5];
                        int worst = bytes[i * 12 + 6];
                        int vendordata = BitConverter.ToInt32(bytes, i * 12 + 7);
                        if (id == 0) continue;

                        var attr = dicDrives[iDriveIndex].Attributes[id];
                        attr.Current = value;
                        attr.Worst = worst;
                        attr.Data = vendordata;
                        attr.IsOK = failureImminent == false;
                    }
                    catch
                    {
                        // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                    }
                }
                iDriveIndex++;
            }

            // retreive threshold values foreach attribute
            searcher.Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictThresholds");
            iDriveIndex = 0;
            foreach (ManagementObject data in searcher.Get())
            {
                Byte[] bytes = (Byte[])data.Properties["VendorSpecific"].Value;
                for (int i = 0; i < 30; ++i)
                {
                    try
                    {

                        int id = bytes[i * 12 + 2];
                        int thresh = bytes[i * 12 + 3];
                        if (id == 0) continue;

                        var attr = dicDrives[iDriveIndex].Attributes[id];
                        attr.Threshold = thresh;
                    }
                    catch
                    {
                        // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                    }
                }

                iDriveIndex++;
            }

            return dicDrives;
        }
    }

    public enum Volumes
    {
        C = 0x2, D = 0x4, E = 0x8, F = 0x10,
        G = 0X20, H = 0X40, I = 0X80, J = 0X100,
        K = 0X200, L = 0x400, M = 0x800, N = 0x1000,
        O = 0x2000, P = 0x4000, Q = 0x8000, R = 0x10000,
        S = 0x20000, T = 0x40000, U = 0x80000, V = 0x100000,
        W = 0x200000, X = 0x400000, Y = 0x800000, Z = 0x1000000
    }
}
