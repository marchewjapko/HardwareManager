using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace DataSource.Usage.Windows.DataRetrieval
{
    [SupportedOSPlatform("windows")]
    internal class DiskInfo
    {
        readonly List<PerformanceCounter> diskUsageCounters = new();

        public DiskInfo()
        {
            var category = new PerformanceCounterCategory("PhysicalDisk");
            string[] instances = category.GetInstanceNames();
            foreach (var instance in instances)
            {
                if (instance == "_Total")
                    continue;
                diskUsageCounters.Add(new PerformanceCounter("PhysicalDisk", "% Disk Time", instance));
            }
            foreach (var counter in diskUsageCounters)
            {
                counter.NextValue();
            }
        }

        internal void UpdateDiskInstances()
        {
            var category = new PerformanceCounterCategory("PhysicalDisk");
            var instancesNew = category.GetInstanceNames().Where(x => x != "_Total").OrderBy(x => x).ToList();
            var instancesOld = diskUsageCounters.Select(x => x.InstanceName).OrderBy(x => x).ToList();
            if (!instancesOld.SequenceEqual(instancesNew))
            {
                var instancesToRemove = instancesOld.Except(instancesNew).ToArray();
                var instancesToAdd = instancesNew.Except(instancesOld).ToArray();
                diskUsageCounters.RemoveAll(x => instancesToRemove.Contains(x.InstanceName));
                if (instancesToAdd.Length > 0)
                {
                    foreach (var instance in instancesToAdd)
                    {
                        if (instance == "_Total")
                            continue;
                        diskUsageCounters.Add(new PerformanceCounter("PhysicalDisk", "% Disk Time", instance));
                    }
                    foreach (var counter in diskUsageCounters)
                    {
                        counter.NextValue();
                    }
                }
            }
        }

        internal List<(string name, float usage)> GetDiskUsage()
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
