using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SystemMonitor.Core.Domain;

namespace HardwareMonitor.Core.Domain
{
    public class SystemSpecs
    {
        public int Id { get; set; }
        public string OsNameVersion { get; set; }
        public string CpuInfo { get; set; }
        public int CpuCores { get; set; }
        public double TotalMemory { get; set; }
        public ICollection<NetworkSpecs> NetworkSpecs { get; set; }
        public ICollection<DiskSpecs> DiskSpecs { get; set; }

        public int SystemReadingId { get; set; }
        [ForeignKey("SystemReadingId")]
        public SystemReading SystemReading { get; set; }
    }
}
