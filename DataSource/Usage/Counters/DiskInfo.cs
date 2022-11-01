using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace DataSource.Counters
{
    [SupportedOSPlatform("windows"), SupportedOSPlatform("linux")]
    internal class DiskInfo
    {
        readonly List<PerformanceCounter> diskUsageCounters = new();
        private string cpuReadingsLinux;

        public DiskInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var category = new PerformanceCounterCategory("PhysicalDisk");
                string[] instances = category.GetInstanceNames();
                foreach (var instance in instances)
                {
                    if (instance == "_Total")
                        continue;
                    diskUsageCounters.Add(new PerformanceCounter("PhysicalDisk", "% Disk Time", instance));
                }
                foreach (var counter in diskUsageCounters)
                {
                    counter.NextValue();
                }
            }
        }

        [SupportedOSPlatform("windows")]
        internal void UpdateDiskInstances()
        {
            var category = new PerformanceCounterCategory("PhysicalDisk");
            var instancesNew = category.GetInstanceNames().Where(x => x != "_Total").OrderBy(x => x).ToList();
            var instancesOld = diskUsageCounters.Select(x => x.InstanceName).OrderBy(x => x).ToList();
            if (!instancesOld.SequenceEqual(instancesNew))
            {
                var instancesToRemove = instancesOld.Except(instancesNew).ToArray();
                var instancesToAdd = instancesNew.Except(instancesOld).ToArray();
                diskUsageCounters.RemoveAll(x => instancesToRemove.Contains(x.InstanceName));
                if (instancesToAdd.Length > 0)
                {
                    foreach (var instance in instancesToAdd)
                    {
                        if (instance == "_Total")
                            continue;
                        diskUsageCounters.Add(new PerformanceCounter("PhysicalDisk", "% Disk Time", instance));
                    }
                    foreach (var counter in diskUsageCounters)
                    {
                        counter.NextValue();
                    }
                }
            }
        }

        internal List<(string name, float usage)> GetDiskUsage()
        {
            List<(string, float)> usage = new();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                foreach (var counter in diskUsageCounters)
                {
                    usage.Add((counter.InstanceName, counter.NextValue()));
                }
            }
            else
            {
                var lines = cpuReadingsLinux.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                for (int i = 2; i < lines.Length; i++)
                {
                    var instanceName = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries)[0];
                    var instanceUsage = float.Parse(lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries)[^1].Replace(',', '.'));
                    usage.Add((instanceName, instanceUsage));
                }
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
