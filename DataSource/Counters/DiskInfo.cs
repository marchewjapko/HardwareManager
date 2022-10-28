using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource.Counters
{
    [SupportedOSPlatform("windows")]
    public class DiskInfo
    {
        List<PerformanceCounter> diskUsageCounters = new();

        public DiskInfo()
        {
            var category = new PerformanceCounterCategory("PhysicalDisk");
            string[] instances = category.GetInstanceNames();
            foreach (var instance in instances)
            {
                if(instance == "_Total")
                    continue;
                diskUsageCounters.Add(new PerformanceCounter("PhysicalDisk", "% Disk Time", instance));
            }
            foreach (var counter in diskUsageCounters)
            {
                counter.NextValue();
            }
        }

        public List<(string name, float usage)> GetDiskUsage()
        {
            List<(string, float)> usage = new();
            foreach (var counter in diskUsageCounters)
            {
                usage.Add((counter.InstanceName, counter.NextValue()));
            }
            return usage.OrderBy(x => x.Item1).ToList();
        }
    }
}
