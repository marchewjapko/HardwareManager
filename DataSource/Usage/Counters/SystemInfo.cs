using System.Diagnostics;
using System.Runtime.Versioning;
using System.Runtime.InteropServices;

namespace DataSource.Counters
{
    [SupportedOSPlatform("windows"), SupportedOSPlatform("linux")]
    internal class SystemInfo
    {
        readonly PerformanceCounter systemUptimeCounter;

        public SystemInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                systemUptimeCounter = new PerformanceCounter("System", "System Up Time");
                systemUptimeCounter.NextValue();
            }
        }
        internal float GetSystemUptime()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return systemUptimeCounter.NextValue();
            }
            else
            {
                var command = new ProcessStartInfo("cat")
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"cat /proc/uptime\"",
                    RedirectStandardOutput = true
                };
                var commandOutput = "";
                using (var process = Process.Start(command))
                {
                    if (process == null)
                    {
                        throw new Exception("Error when executing process: " + command.Arguments);
                    }
                    commandOutput = process.StandardOutput.ReadToEnd();
                }
                return float.Parse(commandOutput.Split(" ")[0]);
            }
        }
    }
}
