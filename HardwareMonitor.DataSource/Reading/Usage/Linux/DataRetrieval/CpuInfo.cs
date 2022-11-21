using SharedObjects;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource.Usage.Linux.DataRetrieval
{
    [SupportedOSPlatform("linux")]
    internal class CpuInfo
    {
        private string cpuReadingsLinux;

        internal double GetCpuTotalUsage()
        {
            var lines = cpuReadingsLinux.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var usage = 100.0 - Convert.ToDouble(lines[^(Environment.ProcessorCount + 1)].Split(" ", StringSplitOptions.RemoveEmptyEntries)[^1].Replace(',', '.'));
            return usage;
        }

        internal List<StringDoublePair> GetCpuPerCoreUsage()
        {
            var usage = new List<StringDoublePair>();
            var lines = cpuReadingsLinux.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            for (int i = lines.Length - Environment.ProcessorCount; i < lines.Length; i++)
            {
                var instanceName = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];
                var instanceUsage = 100.0 - Convert.ToDouble(lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries)[^1].Replace(',', '.'));
                usage.Add(new StringDoublePair()
                {
                    Item1 = instanceName,
                    Item2 = instanceUsage
                });
            }
            return usage.OrderBy(x => x.Item1).ToList();
        }

        internal void UpdateCpuReadingsLinux()
        {
            var command = new ProcessStartInfo("mpstat")
            {
                FileName = "/bin/bash",
                Arguments = "-c \"mpstat -P ALL 1 1\"",
                RedirectStandardOutput = true
            };
            using var process = Process.Start(command);
            if (process == null)
            {
                throw new Exception("Error when executing process: " + command.Arguments);
            }
            cpuReadingsLinux = process.StandardOutput.ReadToEnd();
        }
    }
}
