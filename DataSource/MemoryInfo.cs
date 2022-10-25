
using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource
{
    public static class MemoryInfo
    {
        [SupportedOSPlatform("windows")]
        public static double GetRemainingMemory()
        {
            PerformanceCounter counter = new PerformanceCounter("Memory", "Available MBytes");
            return (double)counter.NextValue();
        }
    }
}
