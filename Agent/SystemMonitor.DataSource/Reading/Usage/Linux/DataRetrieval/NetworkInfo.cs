using DataSource.Helpers;
using System.Diagnostics;
using System.Runtime.Versioning;
using SystemMonitor.SharedObjects;

namespace DataSource.Usage.Linux.DataRetrieval
{
    [SupportedOSPlatform("linux")]
    internal class NetworkInfo
    {
        string networkReadingsLinux;

        internal List<CreateNetworkUsage> GetNetworkInfo()
        {
            var networkUsage = new List<CreateNetworkUsage>();
            var splitReadings = networkReadingsLinux.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var adapters = LinuxNetworkHelpers.GetAllNetworkAdapters();
            for (int i = 0; i < adapters.Count * 2; i += 2)
            {
                var received = Convert.ToDouble(splitReadings[2].Split(" ", StringSplitOptions.RemoveEmptyEntries)[i]);
                var sent = Convert.ToDouble(splitReadings[2].Split(" ", StringSplitOptions.RemoveEmptyEntries)[i + 1]);
                networkUsage.Add(new CreateNetworkUsage()
                {
                    AdapterName = adapters[i / 2],
                    BytesReceived = received,
                    BytesSent = sent
                });
            }
            return networkUsage;
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
