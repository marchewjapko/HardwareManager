using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource.Counters
{
    [SupportedOSPlatform("windows")]
    public class NetworkInfo
    {
        List<PerformanceCounter> bandwidthCounters = new List<PerformanceCounter>();
        List<PerformanceCounter> bytesReceivedCounters = new List<PerformanceCounter>();
        List<PerformanceCounter> bytesSentCounters = new List<PerformanceCounter>();

        public NetworkInfo()
        {
            var category = new PerformanceCounterCategory("Network Interface");
            string[] instances = category.GetInstanceNames();
            foreach (var instance in instances)
            {
                bandwidthCounters.Add(new PerformanceCounter("Network Interface", "Current Bandwidth", instance));
                bytesReceivedCounters.Add(new PerformanceCounter("Network Interface", "Bytes Received/sec", instance));
                bytesSentCounters.Add(new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance));
            }
            foreach (var counter in bandwidthCounters)
            {
                counter.NextValue();
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

        public List<(string name, float bandwidth)> GetBandwidths()
        {
            List<(string, float)> bandwidths = new();
            foreach (var counter in bandwidthCounters)
            {
                bandwidths.Add((counter.InstanceName, counter.NextValue()));
            }
            return bandwidths.OrderBy(x => x.Item1).ToList();
        }

        public List<(string name, float bytes)> GetBytesReceived()
        {
            List<(string, float)> bytesReceived = new();
            foreach (var counter in bytesReceivedCounters)
            {
                bytesReceived.Add((counter.InstanceName, counter.NextValue()));
            }
            return bytesReceived.OrderBy(x => x.Item1).ToList();
        }

        public List<(string name, float bytes)> GetBytesSent()
        {
            List<(string, float)> bytesSent = new();
            foreach (var counter in bytesSentCounters)
            {
                bytesSent.Add((counter.InstanceName, counter.NextValue()));
            }
            return bytesSent.OrderBy(x => x.Item1).ToList();
        }
    }
}
