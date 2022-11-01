using System.Diagnostics;
using System.Runtime.Versioning;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using DataSource.Helpers;

namespace DataSource.Counters
{
    [SupportedOSPlatform("windows"), SupportedOSPlatform("linux")]
    internal class NetworkInfo
    {
        readonly List<PerformanceCounter> bytesReceivedCounters = new();
        readonly List<PerformanceCounter> bytesSentCounters = new();
        string networkReadingsLinux;

        public NetworkInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var category = new PerformanceCounterCategory("Network Interface");
                string[] instances = category.GetInstanceNames();
                foreach (var instance in instances)
                {
                    bytesReceivedCounters.Add(new PerformanceCounter("Network Interface", "Bytes Received/sec", instance));
                    bytesSentCounters.Add(new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance));
                }
                foreach (var counter in bytesReceivedCounters)
                {
                    counter.NextValue();
                }
                foreach (var counter in bytesSentCounters)
                {
                    counter.NextValue();
                }
            }
        }

        [SupportedOSPlatform("windows")]
        internal void UpdateNetworkInstances()
        {
            var category = new PerformanceCounterCategory("Network Interface");
            var instancesNew = category.GetInstanceNames().Where(x => x != "_Total").OrderBy(x => x).ToList();
            var instancesOld = bytesReceivedCounters.Select(x => x.InstanceName).OrderBy(x => x).ToList();
            if (!instancesOld.SequenceEqual(instancesNew))
            {
                var instancesToRemove = instancesOld.Except(instancesNew).ToArray();
                var instancesToAdd = instancesNew.Except(instancesOld).ToArray();
                bytesReceivedCounters.RemoveAll(x => instancesToRemove.Contains(x.InstanceName));
                bytesSentCounters.RemoveAll(x => instancesToRemove.Contains(x.InstanceName));
                if (instancesToAdd.Length > 0)
                {
                    foreach (var instance in instancesToAdd)
                    {
                        if (instance == "_Total")
                            continue;
                        bytesReceivedCounters.Add(new PerformanceCounter("PhysicalDisk", "% Disk Time", instance));
                        bytesSentCounters.Add(new PerformanceCounter("PhysicalDisk", "% Disk Time", instance));
                    }
                    foreach (var counter in bytesReceivedCounters)
                    {
                        counter.NextValue();
                    }
                    foreach (var counter in bytesSentCounters)
                    {
                        counter.NextValue();
                    }
                }
            }
        }

        internal List<(string name, float bytes)> GetBytesReceived()
        {
            var bytesReceived = new List<(string, float)>();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                foreach (var counter in bytesReceivedCounters)
                {
                    bytesReceived.Add((counter.InstanceName, counter.NextValue()));
                }
            }
            else
            {
                var splitReadings = networkReadingsLinux.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                var adapters = LinuxNetworkHelpers.GetAllNetworkAdapters();
                for(int i = 0; i < adapters.Count * 2; i+=2) {
                    var usage = float.Parse(splitReadings[2].Split(" ", StringSplitOptions.RemoveEmptyEntries)[i]) * 1024;
                    bytesReceived.Add((adapters[i/2], usage));
                }
            }
            return bytesReceived;
        }

        internal List<(string name, float bytes)> GetBytesSent()
        {
            var bytesSent = new List<(string, float)>();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                foreach (var counter in bytesSentCounters)
                {
                    bytesSent.Add((counter.InstanceName, counter.NextValue()));
                }
            }
            else
            {
                var splitReadings = networkReadingsLinux.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                var adapters = LinuxNetworkHelpers.GetAllNetworkAdapters();
                for(int i = 0; i < adapters.Count * 2; i+=2) {
                    var usage = float.Parse(splitReadings[2].Split(" ", StringSplitOptions.RemoveEmptyEntries)[i+1]) * 1024;
                    bytesSent.Add((adapters[i/2], usage));
                }
            }
            return bytesSent;
        }

        internal void UpdateNetworkReadingsLinux()
        {
            var adapters = LinuxNetworkHelpers.GetAllNetworkAdapters();
            var command = new ProcessStartInfo("ifstat")
            {
                FileName = "/bin/bash",
                Arguments = "-c \"ifstat -i " + string.Join(',', adapters) + " 1 1\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            using var process = Process.Start(command);
            if (process == null)
            {
                throw new Exception("Error when executing process: " + command.Arguments);
            }
            networkReadingsLinux = process.StandardOutput.ReadToEnd();
        }
    }
}
