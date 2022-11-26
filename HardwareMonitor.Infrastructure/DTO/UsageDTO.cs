using HardwareMonitor.Infrastructure.DTO;
using SharedObjects;

namespace HardwareMonitor.Infrastructure.DTO
{
    public class UsageDTO
    {
        public double CpuTotalUsage { get; set; }
        public List<StringDoublePair> CpuPerCoreUsage { get; set; }
        public List<StringDoublePair> DiskUsage { get; set; }
        public double MemoryUsage { get; set; }
        public List<StringDoublePair> BytesReceived { get; set; }
        public List<StringDoublePair> BytesSent { get; set; }
        public double SystemUptime { get; set; }
    }
}
