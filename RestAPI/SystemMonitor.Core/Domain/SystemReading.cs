using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareMonitor.Core.Domain
{
    public class SystemReading
    {
        public int Id { get; set; }
        public int SystemInfoId { get; set; }
        [ForeignKey("SystemInfoId")]
        public SystemInfo SystemInfo { get; set; }
        public Usage Usage { get; set; }
        public SystemSpecs SystemSpecs { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
