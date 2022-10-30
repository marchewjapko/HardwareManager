using System.Diagnostics;
using System.Runtime.Versioning;
using System.Runtime.InteropServices;

namespace DataSource.Counters
{
    [SupportedOSPlatform("windows"), SupportedOSPlatform("linux")]
    public class CpuInfo
    {
        PerformanceCounter cpuTotalCounter;
        List<PerformanceCounter> cpuPerCoreCounters = new List<PerformanceCounter>();
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

        public float GetCpuTotalUsage()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return cpuTotalCounter.NextValue();
            }
            var lines = cpuReadingsLinux.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var usage = (float)100.0 - float.Parse(lines[^(Environment.ProcessorCount + 1)].Split(" ", StringSplitOptions.RemoveEmptyEntries)[^1].Replace(',', '.'));
            return usage;
        }

        public List<(string name, float usage)> GetCpuPerCoreUsage()
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
                for (int i = lines.Count() - Environment.ProcessorCount; i < lines.Count(); i++)
                {
                    var instanceName = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];
                    var instanceUsage = (float)100.0 - float.Parse(lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries)[^1].Replace(',', '.'));
                    usage.Add((instanceName, instanceUsage));
                }
            }
            return usage.OrderBy(x => x.Item1).ToList();
        }

        [SupportedOSPlatform("linux")]
        public void UpdateCpuReadingsLinux()
        {
            var command = new ProcessStartInfo("mpstat");
            command.FileName = "/bin/bash";
            command.Arguments = "-c \"mpstat -P ALL 1 1\"";
            command.RedirectStandardOutput = true;
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