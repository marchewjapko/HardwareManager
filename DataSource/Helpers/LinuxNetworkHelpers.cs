using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource.Helpers
{
    internal static class LinuxNetworkHelpers
    {
        internal static List<string> GetAllNetworkAdapters()
        {
            var command = new ProcessStartInfo("tcpdump")
            {
                FileName = "/bin/bash",
                Arguments = "-c \"tcpdump --list-interfaces\"",
                RedirectStandardOutput = true
            };
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
            var networkAdapters = commandOutput.Split("\n", StringSplitOptions.RemoveEmptyEntries).Where(x => !omitInterfaces.Any(y => x.Contains(y)) && x.Contains("Running")).ToList();
            for (int i = 0; i < networkAdapters.Count; i++)
            {
                networkAdapters[i] = networkAdapters[i][(networkAdapters[i].IndexOf('.') + 1)..].Split(" ")[0];
            }
            return networkAdapters;
        }
        internal static string GetEthernetBandwidth(string adapter)
        {
            var command = new ProcessStartInfo("ethtool " + adapter + " | grep Speed")
            {
                FileName = "/bin/bash",
                Arguments = String.Format("-c \"ethtool {0} | grep Speed\"", adapter),
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
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
        internal static string GetWirelessBandwidth(string adapter)
        {
            var command = new ProcessStartInfo("iwlist")
            {
                FileName = "/bin/bash",
                Arguments = String.Format("-c \"iwlist {0} rate\"", adapter),
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
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
    }
}
