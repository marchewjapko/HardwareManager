using System.Text;

namespace SharedObjects
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
        public DateTime Timestamp { get; set; }

        public UsageDTO(
            double cpuTotalUsage,
            List<StringDoublePair> cpuPerCoreUsage,
            List<StringDoublePair> diskUsage,
            double memoryUsage,
            List<StringDoublePair> bytesReceived,
            List<StringDoublePair> bytesSent,
            double systemUptime,
            DateTime timestamp
        )
        {
            CpuTotalUsage = cpuTotalUsage;
            CpuPerCoreUsage = cpuPerCoreUsage;
            DiskUsage = diskUsage;
            MemoryUsage = memoryUsage;
            BytesReceived = bytesReceived;
            BytesSent = bytesSent;
            SystemUptime = systemUptime;
            Timestamp = timestamp;
        }

        public override string ToString()
        {
            StringBuilder result = new();
            TimeSpan time = TimeSpan.FromSeconds(SystemUptime);

            result.Append("Total CPU usage: " + Math.Round(CpuTotalUsage, 2) + "%\n");
            result.Append("Per core CPU usage: \n");
            foreach (var pair in CpuPerCoreUsage)
            {
                result.Append("\tCore #" + pair.Item1 + " - " + Math.Round(pair.Item2, 2) + "%\n");
            }

            result.Append("Disk usage: \n");
            foreach (var pair in DiskUsage)
            {
                result.Append("\tDisk: " + pair.Item1 + " - " + Math.Round(pair.Item2, 2) + "%\n");
            }

            result.Append("Available memory: " + MemoryUsage + " MB\n");

            result.Append("Network adapters bytes received:\n");
            foreach (var pair in BytesReceived)
            {
                result.Append("\tAdapter: " + pair.Item1 + " - " + pair.Item2 + " B/sec\n");
            }
            result.Append("Network adapters bytes sent:\n");
            foreach (var pair in BytesSent)
            {
                result.Append("\tAdapter: " + pair.Item1 + " - " + pair.Item2 + " B/sec\n");
            }

            result.Append("System uptime: " + time.ToString(@"hh\:mm\:ss\:fff") + "\n");
            result.Append("Timestamp: " + Timestamp);
            return result.ToString();
        }
    }
}
