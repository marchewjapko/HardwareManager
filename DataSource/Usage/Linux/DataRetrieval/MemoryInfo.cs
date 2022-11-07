using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource.Usage.Linux.DataRetrieval
{
    [SupportedOSPlatform("linux")]
    internal class MemoryInfo
    {
        internal float GetRemainingMemory()
        {
            var command = new ProcessStartInfo("free")
            {
                FileName = "/bin/bash",
                Arguments = "-c \"free -m\"",
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
            var usage = commandOutput.Split("\n")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)[^1];
            return float.Parse(usage);
        }
    }
}
