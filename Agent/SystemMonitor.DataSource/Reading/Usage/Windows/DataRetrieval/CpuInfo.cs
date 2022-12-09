using SharedObjects;
using System.Diagnostics;
using System.Runtime.Versioning;
using SystemMonitor.SharedObjects;

namespace DataSource.Usage.Windows.DataRetrieval
{
    [SupportedOSPlatform("windows")]
    internal class CpuInfo
    {
        readonly PerformanceCounter cpuTotalCounter;
        readonly List<PerformanceCounter> cpuPerCoreCounters = new();

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

        internal float GetCpuTotalUsage()
        {
            return cpuTotalCounter.NextValue();
        }

        internal List<CreateCpuPerCoreUsage> GetCpuPerCoreUsage()
        {
            var usage = new List<CreateCpuPerCoreUsage>();
            foreach (var counter in cpuPerCoreCounters)
            {
                usage.Add(new CreateCpuPerCoreUsage()
                {
                    Instance = counter.InstanceName,
                    Usage = counter.NextValue()
                });
            }
            return usage.OrderBy(x => x.Instance).ToList();
        }
    }
}