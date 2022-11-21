using SharedObjects;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource.Usage.Linux.DataRetrieval
{
    [SupportedOSPlatform("linux")]
    internal class DiskInfo
    {
        private string cpuReadingsLinux;
        internal List<StringDoublePair> GetDiskUsage()
        {
            List<StringDoublePair> usage = new();
            var lines = cpuReadingsLinux.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 2; i < lines.Length; i++)
            {
                var instanceName = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries)[0];
                var instanceUsage = Convert.ToDouble(lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries)[^1].Replace(',', '.'));
                usage.Add(new StringDoublePair()
                {
                    Item1 = instanceName,
                    Item2 = instanceUsage
                });
            }
            return usage.OrderBy(x => x.Item1).ToList();
        }

        internal void UpdateDiskReadingsLinux()
        {
            var command = new ProcessStartInfo("iostat")
            {
                FileName = "/bin/bash",
                Arguments = "-c \"iostat -dxy 1 1\"",
                RedirectStandardOutput = true
            };
            using (var process = Process.Start(command))
            {
                if (process == null)
                {
                    throw new Exception("Error when executing process: " + command.Arguments);
                }
                cpuReadingsLinux = process.StandardOutput.ReadToEnd();
            }
        }
    }
}
