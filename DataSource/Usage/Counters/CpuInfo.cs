using System.Diagnostics;
using System.Runtime.Versioning;
using System.Runtime.InteropServices;

namespace DataSource.Counters
{
    [SupportedOSPlatform("windows"), SupportedOSPlatform("linux")]
    internal class CpuInfo
    {
        readonly PerformanceCounter cpuTotalCounter;
        readonly List<PerformanceCounter> cpuPerCoreCounters = new();
        private string cpuReadingsLinux;

        public CpuInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                cpuTotalCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                var category = new PerformanceCounterCategory("Processor");
                string[] instances = category.GetInstanceNames();
                foreach (var instance in instances)
                {
                    if (instance == "_Total")
                        continue;
                    cpuPerCoreCounters.Add(new PerformanceCounter("Processor", "% Processor Time", instance));
                }
                foreach (var counter in cpuPerCoreCounters)
                {
                    counter.NextValue();
                }
                cpuTotalCounter.NextValue();
            }
        }

        internal float GetCpuTotalUsage()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return cpuTotalCounter.NextValue();
            }
            else
            {
                var lines = cpuReadingsLinux.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                var usage = (float)100.0 - float.Parse(lines[^(Environment.ProcessorCount + 1)].Split(" ", StringSplitOptions.RemoveEmptyEntries)[^1].Replace(',', '.'));
                return usage;
            }
        }

        internal List<(string name, float usage)> GetCpuPerCoreUsage()
        {
            var usage = new List<(string, float)>();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                foreach (var counter in cpuPerCoreCounters)
                {
                    usage.Add((counter.InstanceName, counter.NextValue()));
                }
            }
            else
            {
                var lines = cpuReadingsLinux.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                for (int i = lines.Length - Environment.ProcessorCount; i < lines.Length; i++)
                {
                    var instanceName = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];
                    var instanceUsage = (float)100.0 - float.Parse(lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries)[^1].Replace(',', '.'));
                    usage.Add((instanceName, instanceUsage));
                }
            }
            return usage.OrderBy(x => x.Item1).ToList();
        }

        [SupportedOSPlatform("linux")]
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