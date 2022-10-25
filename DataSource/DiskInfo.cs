using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource
{
    public static class DiskInfo
    {
        [SupportedOSPlatform("windows")]
        public static List<(string name, double usage)> GetDiskUsage()
        {
            List<(string, double)> bytesSent = new();
            var category = new PerformanceCounterCategory("PhysicalDisk");
            string[] instances = category.GetInstanceNames();
            foreach (var instance in instances)
            {
                var counter = new PerformanceCounter("PhysicalDisk", "% Disk Time", instance);
                bytesSent.Add((instance, counter.NextValue()));
            }
            return bytesSent;
        }
    }
}
