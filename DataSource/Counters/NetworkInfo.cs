using System.Diagnostics;
using System.Runtime.Versioning;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace DataSource.Counters
{
    [SupportedOSPlatform("windows"), SupportedOSPlatform("linux")]
    public class NetworkInfo
    {
        List<PerformanceCounter> bandwidthCounters = new List<PerformanceCounter>();
        List<PerformanceCounter> bytesReceivedCounters = new List<PerformanceCounter>();
        List<PerformanceCounter> bytesSentCounters = new List<PerformanceCounter>();
        string networkReadingsLinux;


        public NetworkInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
        }

        public List<(string name, float bandwidth)> GetBandwidths()
        {
            var bandwidths = new List<(string, float)>();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                foreach (var counter in bandwidthCounters)
                {
                    bandwidths.Add((counter.InstanceName, counter.NextValue()));
                }
                return bandwidths.OrderBy(x => x.Item1).ToList();
            }
            else
            {
                var adapters = GetAllNetworkAdapters();
                foreach (var adapter in adapters)
                {
                    var usage = GetEthernetAdapterUsage(adapter);
                    if (usage == "")
                    {
                        usage = GetWirelessAdapterUsage(adapter);
                        usage = usage.Split("\n").First(x => x.Contains("Current")).Split(" ", StringSplitOptions.RemoveEmptyEntries).First(x => x.Contains("Rate"));
                        var value = (float)0;
                        if (usage.Contains(':'))
                        {
                            value = float.Parse(usage.Substring(usage.IndexOf(':') + 1)) * 1000000;
                        }
                        else
                        {
                            value = float.Parse(usage.Substring(usage.IndexOf('=') + 1)) * 1000000;
                        }
                        bandwidths.Add((adapter, value));
                    }
                    else
                    {
                        var value = float.Parse(Regex.Replace(usage.Split(" ")[^1], "[^0-9]", "")) * 1000000;
                        bandwidths.Add((adapter, value));
                    }
                }
            }
            return bandwidths;
        }

        public List<(string name, float bytes)> GetBytesReceived()
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
                var adapters = GetAllNetworkAdapters();
                for(int i = 0; i < adapters.Count * 2; i+=2) {
                    var usage = float.Parse(splitReadings[2].Split(" ", StringSplitOptions.RemoveEmptyEntries)[i]) * 1024;
                    bytesReceived.Add((adapters[i/2], usage));
                }
            }
            return bytesReceived;
        }

        public List<(string name, float bytes)> GetBytesSent()
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
                var adapters = GetAllNetworkAdapters();
                for(int i = 0; i < adapters.Count * 2; i+=2) {
                    var usage = float.Parse(splitReadings[2].Split(" ", StringSplitOptions.RemoveEmptyEntries)[i+1]) * 1024;
                    bytesSent.Add((adapters[i/2], usage));
                }
            }
            return bytesSent;
        }

        private bool ContainsAny(string str, string[] searchPhrases)
        {
            foreach (string phrase in searchPhrases)
            {
                if (str.Contains(phrase))
                {
                    return true;
                }
            }
            return false;
        }
        private List<string> GetAllNetworkAdapters()
        {
            var command = new ProcessStartInfo("tcpdump");
            command.FileName = "/bin/bash";
            command.Arguments = "-c \"tcpdump --list-interfaces\"";
            command.RedirectStandardOutput = true;
            var commandOutput = "";
            using (var process = Process.Start(command))
            {
                if (process == null)
                {
                    throw new Exception("Error when executing process: " + command.Arguments);
                }
                commandOutput = process.StandardOutput.ReadToEnd();
            }
            var omitInterfaces = new string[] { "Loopback", "Pseudo-device", "none", "Bluetooth adapter", "Linux netfilter" };
            var networkAdapters = commandOutput.Split("\n", StringSplitOptions.RemoveEmptyEntries).Where(x => !ContainsAny(x, omitInterfaces) && x.Contains("Running")).ToList();
            for (int i = 0; i < networkAdapters.Count(); i++)
            {
                networkAdapters[i] = networkAdapters[i].Substring(networkAdapters[i].IndexOf('.') + 1).Split(" ")[0];
            }
            return networkAdapters;
        }
        private string GetEthernetAdapterUsage(string adapter)
        {
            var command = new ProcessStartInfo("ethtool " + adapter + " | grep Speed");
            command.FileName = "/bin/bash";
            command.Arguments = String.Format("-c \"ethtool {0} | grep Speed\"", adapter);
            command.RedirectStandardOutput = true;
            command.RedirectStandardError = true;
            var commandOutput = "";
            using (var process = Process.Start(command))
            {
                if (process == null)
                {
                    throw new Exception("Error when executing process: " + command.Arguments);
                }
                commandOutput = process.StandardOutput.ReadToEnd();
            }
            if (String.IsNullOrEmpty(commandOutput))
            {
                return "";
            }
            return commandOutput;
        }
        private string GetWirelessAdapterUsage(string adapter)
        {
            var command = new ProcessStartInfo("iwlist");
            command.FileName = "/bin/bash";
            command.Arguments = String.Format("-c \"iwlist {0} rate\"", adapter);
            command.RedirectStandardOutput = true;
            command.RedirectStandardError = true;
            var commandOutput = "";
            using (var process = Process.Start(command))
            {
                if (process == null)
                {
                    throw new Exception("Error when executing process: " + command.Arguments);
                }
                commandOutput = process.StandardOutput.ReadToEnd();
            }
            return commandOutput;
        }
        public void UpdateNetworkReadingsLinux()
        {
            var adapters = GetAllNetworkAdapters();
            var command = new ProcessStartInfo("ifstat");
            command.FileName = "/bin/bash";
            command.Arguments = "-c \"ifstat -i " + string.Join(',', adapters) + " 1 1\"";
            command.RedirectStandardOutput = true;
            command.RedirectStandardError = true;
            using (var process = Process.Start(command))
            {
                if (process == null)
                {
                    throw new Exception("Error when executing process: " + command.Arguments);
                }
                networkReadingsLinux = process.StandardOutput.ReadToEnd();
            }
        }
    }
}
