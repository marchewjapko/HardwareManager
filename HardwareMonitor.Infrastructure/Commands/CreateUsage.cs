using SharedObjects;

namespace HardwareMonitor.Infrastructure.Commands
{
    public class CreateUsage
    {
        public double CpuTotalUsage { get; set; }
        public List<StringDoublePair> CpuPerCoreUsage { get; set; }
        public List<StringDoublePair> DiskUsage { get; set; }
        public double MemoryUsage { get; set; }
        public List<StringDoublePair> BytesReceived { get; set; }
        public List<StringDoublePair> BytesSent { get; set; }
        public double SystemUptime { get; set; }
        public DateTime Timestamp { get; set; }
    };
}
