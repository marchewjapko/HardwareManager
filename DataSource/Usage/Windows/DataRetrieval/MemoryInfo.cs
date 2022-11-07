using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource.Usage.Windows.DataRetrieval
{
    [SupportedOSPlatform("windows")]
    internal class MemoryInfo
    {
        readonly PerformanceCounter memoryCounter;

        public MemoryInfo()
        {
            memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
            memoryCounter.NextValue();
        }
        internal float GetRemainingMemory()
        {
            return memoryCounter.NextValue();
        }
    }
}
