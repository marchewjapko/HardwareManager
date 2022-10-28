using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource.Counters
{
    [SupportedOSPlatform("windows")]
    public class CpuInfo
    {
        PerformanceCounter cpuTotalCounter;
        List<PerformanceCounter> cpuPerCoreCounters = new List<PerformanceCounter>();

        public CpuInfo()
        {
            cpuTotalCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var category = new PerformanceCounterCategory("Processor");
            string[] instances = category.GetInstanceNames();
            foreach (var instance in instances)
            {
                if (instance == "_Total")
                    continue;
                cpuPerCoreCounters.Add(new PerformanceCounter("Processor", "% Processor Time", instance));
            }
            foreach (var counter in cpuPerCoreCounters)
            {
                counter.NextValue();
            }
            cpuTotalCounter.NextValue();
        }

        public float GetCpuTotalUsage()
        {
            return cpuTotalCounter.NextValue();
        }

        public List<(string name, float usage)> GetCpuPerCoreUsage()
        {
            List<(string, float)> usage = new();
            foreach (var counter in cpuPerCoreCounters)
            {
                usage.Add((counter.InstanceName, counter.NextValue()));
            }
            return usage.OrderBy(x => x.Item1).ToList();
        }
    }
}