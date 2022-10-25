using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource
{
    public static class NetworkInfo
    {
        [SupportedOSPlatform("windows")]
        public static List<(string name, double bandwidth)> GetBandwidth()
        {
            List<(string, double)> bandwidths = new();
            var category = new PerformanceCounterCategory("Network Interface");
            string[] instances = category.GetInstanceNames();
            foreach (var instance in instances)
            {
                var counter = new PerformanceCounter("Network Interface", "Current Bandwidth", instance);
                bandwidths.Add((instance, counter.RawValue));
            }
            return bandwidths;
        }

        [SupportedOSPlatform("windows")]
        public static List<(string name, double bytes)> GetBytesReceived()
        {
            List<(string, double)> bytesReceived = new();
            var category = new PerformanceCounterCategory("Network Interface");
            string[] instances = category.GetInstanceNames();
            foreach (var instance in instances)
            {
                var counter = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
                bytesReceived.Add((instance, counter.NextValue()));
            }
            return bytesReceived;
        }

        [SupportedOSPlatform("windows")]
        public static List<(string name, double bytes)> GetBytesSent()
        {
            List<(string, double)> bytesSent = new();
            var category = new PerformanceCounterCategory("Network Interface");
            string[] instances = category.GetInstanceNames();
            foreach (var instance in instances)
            {
                var counter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
                bytesSent.Add((instance, counter.NextValue()));
            }
            return bytesSent;
        }
    }
}
