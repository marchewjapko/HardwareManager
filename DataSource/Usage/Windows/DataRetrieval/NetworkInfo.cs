using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource.Usage.Windows.DataRetrieval
{
    [SupportedOSPlatform("windows")]
    internal class NetworkInfo
    {
        readonly List<PerformanceCounter> bytesReceivedCounters = new();
        readonly List<PerformanceCounter> bytesSentCounters = new();

        public NetworkInfo()
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
            foreach (var counter in bytesReceivedCounters)
            {
                bytesReceived.Add((counter.InstanceName, counter.NextValue()));
            }
            return bytesReceived;
        }

        internal List<(string name, float bytes)> GetBytesSent()
        {
            var bytesSent = new List<(string, float)>();
            foreach (var counter in bytesSentCounters)
            {
                bytesSent.Add((counter.InstanceName, counter.NextValue()));
            }
            return bytesSent;
        }
    }
}
