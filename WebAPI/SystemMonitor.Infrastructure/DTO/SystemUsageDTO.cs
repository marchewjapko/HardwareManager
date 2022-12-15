using SystemMonitor.Infrastructure.DTO;

namespace SystemMonitor.Infrastructure.DTO
{
    public class SystemUsageDTO
    {
        public double CpuTotalUsage { get; set; }
        public List<CpuPerCoreUsageDTO> CpuPerCoreUsage { get; set; }
        public List<DiskUsageDTO> DiskUsage { get; set; }
        public double MemoryUsage { get; set; }
        public List<NetworkUsageDTO> NetworkUsage { get; set; }
        public double SystemUptime { get; set; }
    }
}
