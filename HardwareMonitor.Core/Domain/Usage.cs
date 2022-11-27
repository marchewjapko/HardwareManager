using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareMonitor.Core.Domain
{
    public class Usage
    {
        public int Id { get; set; }
        public double CpuTotalUsage { get; set; }
        public string CpuPerCoreUsage { get; set; }
        public string DiskUsage { get; set; }
        public double MemoryUsage { get; set; }
        public string BytesReceived { get; set; }
        public string BytesSent { get; set; }
        public double SystemUptime { get; set; }

        public int SystemReadingId { get; set; }
        [ForeignKey("SystemReadingId")]
        public SystemReading SystemReading { get; set; }
    }
}
