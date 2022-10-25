using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource
{
    public static class System
    {
        [SupportedOSPlatform("windows")]
        public static double GetSystemUptime()
        {
            var counter = new PerformanceCounter("System", "System Up Time");
            return (double)counter.NextValue();
        }

        [SupportedOSPlatform("windows")]
        public static double GetSystemCalls()
        {
            var counter = new PerformanceCounter("System", "System Calls/sec");
            return (double)counter.NextValue();
        }
    }
}
