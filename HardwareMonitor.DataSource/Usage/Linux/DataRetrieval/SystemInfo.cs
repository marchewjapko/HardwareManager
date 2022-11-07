using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource.Usage.Linux.DataRetrieval
{
    [SupportedOSPlatform("linux")]
    internal class SystemInfo
    {
        internal float GetSystemUptime()
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
