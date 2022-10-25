using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource
{
    public static class CPUInfo
    {
        [SupportedOSPlatform("windows")]
        public static double GetCpuTotalUsage()
        {
            var counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            return (double)counter.NextValue();
        }

        [SupportedOSPlatform("windows")]
        public static List<(string name, double usage)> GetCpuPerCoreUsage()
        {
            List<(string, double)> usage = new();
            var category = new PerformanceCounterCategory("Processor");
            string[] instances = category.GetInstanceNames();
            foreach(var instance in instances)
            {
                if(instance == "_Total")
                {
                    continue;
                }
                var counter = new PerformanceCounter("Processor", "% Processor Time", instance);
                usage.Add((instance, counter.NextValue()));
            }
            return usage;
        }
    }
}