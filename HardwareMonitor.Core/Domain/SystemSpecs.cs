using System;
using System.Collections.Generic;

namespace HardwareMonitor.Core.Domain
{
    public class SystemSpecs
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public string OsNameVersion { get; set; }
        public string CpuInfo { get; set; }
        public int CpuCores { get; set; }
        public double TotalMemory { get; set; }
        public string NetworkAdapters { get; set; }
        public string Disks { get; set; }

        public int SystemReadingId { get; set; }
        public SystemReading SystemReading { get; set; }
    }
}
