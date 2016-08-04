using System.Collections.Generic;

namespace CommonUtilities.PerformanceRelated
{
    /// <summary>
    /// Tested against Crystal Disk Info 5.3.1 and HD Tune Pro 3.5 on 15 Feb 2013.
    /// Findings; I do not trust the individual smart register "OK" status reported back frm the drives.
    /// I have tested faulty drives and they return an OK status on nearly all applications except HD Tune. 
    /// After further research I see HD Tune is checking specific attribute values against their thresholds
    /// and and making a determination of their own (which is good) for whether the disk is in good condition or not.
    /// I recommend whoever uses this code to do the same. For example -->
    /// "Reallocated sector count" - the general threshold is 36, but even if 1 sector is reallocated I want to know about it and it should be flagged.   
    /// </summary>
    public class DiskSmartInfo
    {

        public int Index { get; set; }
        public bool IsOK { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }

        public ulong Size { get; set; }
        public string Serial { get; set; }
        public Dictionary<int, SmartInfo> Attributes = new Dictionary<int, SmartInfo>() {
                {0x00, new SmartInfo("Invalid")},
                {0x01, new SmartInfo("Raw read error rate")},
                {0x02, new SmartInfo("Throughput performance")},
                {0x03, new SmartInfo("Spinup time")},
                {0x04, new SmartInfo("Start/Stop count")},
                {0x05, new SmartInfo("Reallocated sector count")},
                {0x06, new SmartInfo("Read channel margin")},
                {0x07, new SmartInfo("Seek error rate")},
                {0x08, new SmartInfo("Seek timer performance")},
                {0x09, new SmartInfo("Power-on hours count")},
                {0x0A, new SmartInfo("Spinup retry count")},
                {0x0B, new SmartInfo("Calibration retry count")},
                {0x0C, new SmartInfo("Power cycle count")},
                {0x0D, new SmartInfo("Soft read error rate")},
                {0xB8, new SmartInfo("End-to-End error")},
                {0xBE, new SmartInfo("Airflow Temperature")},
                {0xBF, new SmartInfo("G-sense error rate")},
                {0xC0, new SmartInfo("Power-off retract count")},
                {0xC1, new SmartInfo("Load/Unload cycle count")},
                {0xC2, new SmartInfo("HDD temperature")},
                {0xC3, new SmartInfo("Hardware ECC recovered")},
                {0xC4, new SmartInfo("Reallocation count")},
                {0xC5, new SmartInfo("Current pending sector count")},
                {0xC6, new SmartInfo("Offline scan uncorrectable count")},
                {0xC7, new SmartInfo("UDMA CRC error rate")},
                {0xC8, new SmartInfo("Write error rate")},
                {0xC9, new SmartInfo("Soft read error rate")},
                {0xCA, new SmartInfo("Data Address Mark errors")},
                {0xCB, new SmartInfo("Run out cancel")},
                {0xCC, new SmartInfo("Soft ECC correction")},
                {0xCD, new SmartInfo("Thermal asperity rate (TAR)")},
                {0xCE, new SmartInfo("Flying height")},
                {0xCF, new SmartInfo("Spin high current")},
                {0xD0, new SmartInfo("Spin buzz")},
                {0xD1, new SmartInfo("Offline seek performance")},
                {0xDC, new SmartInfo("Disk shift")},
                {0xDD, new SmartInfo("G-sense error rate")},
                {0xDE, new SmartInfo("Loaded hours")},
                {0xDF, new SmartInfo("Load/unload retry count")},
                {0xE0, new SmartInfo("Load friction")},
                {0xE1, new SmartInfo("Load/Unload cycle count")},
                {0xE2, new SmartInfo("Load-in time")},
                {0xE3, new SmartInfo("Torque amplification count")},
                {0xE4, new SmartInfo("Power-off retract count")},
                {0xE6, new SmartInfo("GMR head amplitude")},
                {0xE7, new SmartInfo("Temperature")},
                {0xF0, new SmartInfo("Head flying hours")},
                {0xFA, new SmartInfo("Read error retry rate")},
                /* slot in any new codes you find in here */
            };

    }
}
