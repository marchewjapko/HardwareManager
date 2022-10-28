using System.Text;

namespace DataSource
{
    public class UsageDTO
    {
        private float cpuTotalUsage;
        private List<(string name, float usage)> cpuPerCoreUsage = new List<(string name, float usage)>();
        private List<(string name, float usage)> diskUsage = new List<(string name, float usage)>();
        public float memoryUsage;
        private List<(string name, float bandwidth)> bandwidths = new List<(string name, float usage)>();
        private List<(string name, float bytes)> bytesReceived = new List<(string name, float usage)>();
        private List<(string name, float bytes)> bytesSent = new List<(string name, float usage)>();
        private float systemUptime;
        private float systemCalls;

        public UsageDTO(
            float cpuTotalUsage,
            List<(string name, float usage)> cpuPerCoreUsage,
            List<(string name, float usage)> diskUsage,
            float memoryUsage,
            List<(string name, float bandwidth)> bandwidths,
            List<(string name, float bytes)> bytesReceived,
            List<(string name, float bytes)> bytesSent,
            float systemUptime,
            float systemCalls
        )
        {
            this.cpuTotalUsage = cpuTotalUsage;
            this.cpuPerCoreUsage = cpuPerCoreUsage;
            this.diskUsage = diskUsage;
            this.memoryUsage = memoryUsage;
            this.bandwidths = bandwidths;
            this.bytesReceived = bytesReceived;
            this.bytesSent = bytesSent;
            this.systemUptime = systemUptime;
            this.systemCalls = systemCalls;
        }

        public UsageDTO getSystemUsage()
        {
            return this;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            TimeSpan time = TimeSpan.FromSeconds(systemUptime);

            result.Append("Total CPU usage: " + Math.Round(cpuTotalUsage, 2) + "%\n");
            result.Append("Per core CPU usage: \n");
            foreach(var item in cpuPerCoreUsage)
            {
                result.Append("\tCore #" + item.name + " - " + Math.Round(item.usage, 2) + "%\n");
            }

            result.Append("Disk usage: \n");
            foreach (var item in diskUsage)
            {
                result.Append("\tDisk: " + item.name + " - " + item.usage + "\n");
            }

            result.Append("Available memory: " + memoryUsage + " MB\n");

            result.Append("Network adapters bandwidths:\n");
            foreach(var item in bandwidths)
            {
                result.Append("\tAdapter: " + item.name + " - " + item.bandwidth + " b/sec\n");
            }
            result.Append("Network adapters bytes received:\n");
            foreach (var item in bytesReceived)
            {
                result.Append("\tAdapter: " + item.name + " - " + item.bytes + " B/sec\n");
            }
            result.Append("Network adapters bytes sent:\n");
            foreach (var item in bytesSent)
            {
                result.Append("\tAdapter: " + item.name + " - " + item.bytes + " B/s\n");
            }

            result.Append("System uptime: " + time.ToString(@"hh\:mm\:ss\:fff") + "\n");
            result.Append("System calls: " + systemCalls + " calls/sec");
            return result.ToString();
        }
    }
}
