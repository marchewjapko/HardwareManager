using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareMonitor.Core.Domain
{
    public class SystemSpecs
    {
        public int Id { get; set; }
        public string OsNameVersion { get; set; }
        public string CpuInfo { get; set; }
        public int CpuCores { get; set; }
        public double TotalMemory { get; set; }
        public string NetworkAdapters { get; set; }
        public string Disks { get; set; }

        public int SystemReadingId { get; set; }
        [ForeignKey("SystemReadingId")]
        public SystemReading SystemReading { get; set; }
    }
}
