using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource.Counters
{
    [SupportedOSPlatform("windows")]
    public class SystemInfo
    {
        PerformanceCounter systemUptimeCounter;
        PerformanceCounter systemCallsCounter;
        public SystemInfo()
        {
            systemUptimeCounter = new PerformanceCounter("System", "System Up Time");
            systemCallsCounter = new PerformanceCounter("System", "System Calls/sec");
            systemUptimeCounter.NextValue();
            systemCallsCounter.NextValue();
        }
        public float GetSystemUptime()
        {
            return systemUptimeCounter.NextValue();
        }

        public float GetSystemCalls()
        {
            return systemCallsCounter.NextValue();
        }
    }
}
