using System.Text;

namespace DataSource.Usage
{
    public class UsageDTO
    {
        private readonly float cpuTotalUsage;
        private readonly List<(string name, float usage)> cpuPerCoreUsage = new();
        private readonly List<(string name, float usage)> diskUsage = new();
        private readonly float memoryUsage;
        private readonly List<(string name, float bytes)> bytesReceived = new();
        private readonly List<(string name, float bytes)> bytesSent = new();
        private readonly float systemUptime;

        public UsageDTO(
            float cpuTotalUsage,
            List<(string name, float usage)> cpuPerCoreUsage,
            List<(string name, float usage)> diskUsage,
            float memoryUsage,
            List<(string name, float bytes)> bytesReceived,
            List<(string name, float bytes)> bytesSent,
            float systemUptime
        )
        {
            this.cpuTotalUsage = cpuTotalUsage;
            this.cpuPerCoreUsage = cpuPerCoreUsage;
            this.diskUsage = diskUsage;
            this.memoryUsage = memoryUsage;
            this.bytesReceived = bytesReceived;
            this.bytesSent = bytesSent;
            this.systemUptime = systemUptime;
        }

        public UsageDTO GetSystemUsage()
        {
            return this;
        }

        public override string ToString()
        {
            StringBuilder result = new();
            TimeSpan time = TimeSpan.FromSeconds(systemUptime);

            result.Append("Total CPU usage: " + Math.Round(cpuTotalUsage, 2) + "%\n");
            result.Append("Per core CPU usage: \n");
            foreach (var (name, usage) in cpuPerCoreUsage)
            {
                result.Append("\tCore #" + name + " - " + Math.Round(usage, 2) + "%\n");
            }

            result.Append("Disk usage: \n");
            foreach (var (name, usage) in diskUsage)
            {
                result.Append("\tDisk: " + name + " - " + Math.Round(usage, 2) + "%\n");
            }

            result.Append("Available memory: " + memoryUsage + " MB\n");

            result.Append("Network adapters bytes received:\n");
            foreach (var (name, bytes) in bytesReceived)
            {
                result.Append("\tAdapter: " + name + " - " + bytes + " B/sec\n");
            }
            result.Append("Network adapters bytes sent:\n");
            foreach (var (name, bytes) in bytesSent)
            {
                result.Append("\tAdapter: " + name + " - " + bytes + " B/sec\n");
            }

            result.Append("System uptime: " + time.ToString(@"hh\:mm\:ss\:fff") + "\n");
            return result.ToString();
        }
    }
}
