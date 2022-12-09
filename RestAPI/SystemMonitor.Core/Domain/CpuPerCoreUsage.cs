using HardwareMonitor.Core.Domain;

namespace SystemMonitor.Core.Domain
{
    public class CpuPerCoreUsage
    {
        public int Id { get; set; }
        public string Instance { get; set; }
        public double Usage { get; set; }

        public int SystemUsageId { get; set; }
        public SystemUsage SystemUsage { get; set; }
    }
}
