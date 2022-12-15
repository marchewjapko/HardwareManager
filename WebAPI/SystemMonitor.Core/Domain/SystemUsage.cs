using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemMonitor.Core.Domain
{
    public class SystemUsage
    {
        public int Id { get; set; }
        public double CpuTotalUsage { get; set; }
        public ICollection<CpuPerCoreUsage> CpuPerCoreUsage { get; set; }
        public ICollection<DiskUsage> DiskUsage { get; set; }
        public double MemoryUsage { get; set; }
        public ICollection<NetworkUsage> NetworkUsage { get; set; }
        public double SystemUptime { get; set; }

        public int SystemReadingId { get; set; }
        [ForeignKey("SystemReadingId")]
        public SystemReading SystemReading { get; set; }
    }
}
