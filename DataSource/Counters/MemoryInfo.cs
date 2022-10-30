using System.Diagnostics;
using System.Runtime.Versioning;
using System.Runtime.InteropServices;

namespace DataSource.Counters
{
    [SupportedOSPlatform("windows"), SupportedOSPlatform("linux")]
    public class MemoryInfo
    {
        PerformanceCounter memoryCounter;
        public MemoryInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
                memoryCounter.NextValue();
            }
        }
        public float GetRemainingMemory()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return memoryCounter.NextValue();
            }
            else
            {
                var command = new ProcessStartInfo("free");
                command.FileName = "/bin/bash";
                command.Arguments = "-c \"free -m\"";
                command.RedirectStandardOutput = true;
                var commandOutput = "";
                using (var process = Process.Start(command))
                {
                    if (process == null)
                    {
                        throw new Exception("Error when executing process: " + command.Arguments);
                    }
                    commandOutput = process.StandardOutput.ReadToEnd();
                }
                var usage = commandOutput.Split("\n")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)[^1];
                return float.Parse(usage);
            }
        }
    }
}
