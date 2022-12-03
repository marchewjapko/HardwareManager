using DataSource.Helpers;
using SharedObjects;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataSource.Usage.Linux.DataRetrieval
{
    [SupportedOSPlatform("linux")]
    internal class NetworkInfo
    {
        string networkReadingsLinux;

        internal List<StringDoublePair> GetBytesReceived()
        {
            var bytesReceived = new List<StringDoublePair>();
            var splitReadings = networkReadingsLinux.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var adapters = LinuxNetworkHelpers.GetAllNetworkAdapters();
            for (int i = 0; i < adapters.Count * 2; i += 2)
            {
                var usage = Convert.ToDouble(splitReadings[2].Split(" ", StringSplitOptions.RemoveEmptyEntries)[i]);
                bytesReceived.Add(new StringDoublePair()
                {
                    Item1 = adapters[i/2],
                    Item2 = usage
                });
            }
            return bytesReceived;
        }

        internal List<StringDoublePair> GetBytesSent()
        {
            var bytesSent = new List<StringDoublePair>();
            var splitReadings = networkReadingsLinux.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var adapters = LinuxNetworkHelpers.GetAllNetworkAdapters();
            for (int i = 0; i < adapters.Count * 2; i += 2)
            {
                var usage = Convert.ToDouble(splitReadings[2].Split(" ", StringSplitOptions.RemoveEmptyEntries)[i + 1]);
                bytesSent.Add(new StringDoublePair()
                {
                    Item1 = adapters[i / 2],
                    Item2 = usage
                });
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
