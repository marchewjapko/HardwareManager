using System.Text;
using SystemMonitor.SharedObjects;

namespace SharedObjects
{
    public class CreateSystemUsage
    {
        public double CpuTotalUsage { get; set; }
        public List<CreateCpuPerCoreUsage> CreateCpuPerCoreUsage { get; set; }
        public List<CreateDiskUsage> CreateDiskUsage { get; set; }
        public double MemoryUsage { get; set; }
        public List<CreateNetworkUsage> CreateNetworkUsage { get; set; }
        public double SystemUptime { get; set; }

        public override string ToString()
        {
            StringBuilder result = new();
            TimeSpan time = TimeSpan.FromSeconds(SystemUptime);

            result.Append("Total CPU usage: " + Math.Round(CpuTotalUsage, 2) + "%\n");
            result.Append("Per core CPU usage: \n");
            foreach (var usage in CreateCpuPerCoreUsage)
            {
                result.Append(usage.ToString());
            }

            result.Append("Disk usage: \n");
            foreach (var usage in CreateDiskUsage)
            {
                result.Append(usage.ToString());
            }

            result.Append("Available memory: " + MemoryUsage + " MB\n");

            result.Append("Network usage:\n");
            foreach (var networkUsage in CreateNetworkUsage)
            {
                result.Append(networkUsage.ToString());
            }
            result.Append("System uptime: " + time.ToString(@"hh\:mm\:ss\:fff") + "\n");
            return result.ToString();
        }
    }
}
