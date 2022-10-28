using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource.Counters
{
    [SupportedOSPlatform("windows")]
    public class MemoryInfo
    {
        PerformanceCounter memoryCounter;
        public MemoryInfo()
        {
            memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
            memoryCounter.NextValue();
        }
        public float GetRemainingMemory()
        {
            return memoryCounter.NextValue();
        }
    }
}
