using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource.Usage.Windows.DataRetrieval
{
    [SupportedOSPlatform("windows")]
    internal class SystemInfo
    {
        readonly PerformanceCounter systemUptimeCounter;

        public SystemInfo()
        {
            systemUptimeCounter = new PerformanceCounter("System", "System Up Time");
            systemUptimeCounter.NextValue();
        }
        internal float GetSystemUptime()
        {
            return systemUptimeCounter.NextValue();
        }
    }
}
