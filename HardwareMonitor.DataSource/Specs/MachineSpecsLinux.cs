using DataSource.Helpers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;

namespace DataSource.Specs
{
    [SupportedOSPlatform("linux")]
    public class MachineSpecsLinux
    {
        private readonly string machineName;
        private readonly string osNameVersion;
        private readonly string cpuInfo;
        private readonly int cpuCores;
        private readonly double totalMemory;
        private readonly List<(string name, float bandwidth)> networkAdapters;
        private readonly List<(string name, int size)> physicalDisks;

        public MachineSpecsLinux()
        {
            machineName = Environment.MachineName;
            osNameVersion = RuntimeInformation.OSDescription;
            cpuInfo = GetCpuInfo();
            cpuCores = Environment.ProcessorCount;
            totalMemory = GetTotalMemory();
            networkAdapters = GetNetworkAdapters();
            physicalDisks = GetPhysicalDisks();
        }

        public MachineSpecsLinux GetMachineSpecs()
        {
            return this;
        }

        public override string ToString()
        {
            StringBuilder result = new("Machine name: " + machineName + "\n");
            result.Append("Operating system: " + osNameVersion + "\n");
            result.Append("CPU: " + cpuInfo + "\n");
            result.Append("CPU cores: " + cpuCores + "\n");
            result.Append("Total RAM: " + Math.Round(totalMemory / 1048576, 1) + " GB\n");
            result.Append("Network adapters: " + "\n");
            foreach (var (name, bandwidth) in networkAdapters)
            {
                result.Append("\t Adapter: " + name + " - " + bandwidth + "b/sec \n");
            }
            result.Append("Physical drives: " + "\n");
            foreach (var (name, size) in physicalDisks)
            {
                result.Append("\t Drive: " + name + " - " + size + " GB\n");
            }
            return result.ToString();
        }

        private static string GetCpuInfo()
        {
            var command = new ProcessStartInfo("cat")
            {
                FileName = "/bin/bash",
                Arguments = "-c \"cat /proc/cpuinfo | grep 'model name' | uniq\"",
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
            return commandOutput[(commandOutput.IndexOf(':') + 1)..][1..^1];
        }

        private static double GetTotalMemory()
        {
            var command = new ProcessStartInfo("cat")
            {
                FileName = "/bin/bash",
                Arguments = "-c \"cat /proc/meminfo | grep 'MemTotal'\"",
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
            return Convert.ToDouble(commandOutput.Split(" ", StringSplitOptions.RemoveEmptyEntries)[^2]);
        }

        private static List<(string name, float bandwidth)> GetNetworkAdapters()
        {
            var result = new List<(string, float)>();
            var adapters = LinuxNetworkHelpers.GetAllNetworkAdapters();
            foreach (var adapter in adapters)
            {
                var usage = LinuxNetworkHelpers.GetEthernetBandwidth(adapter);
                if (usage == "")
                {
                    usage = LinuxNetworkHelpers.GetWirelessBandwidth(adapter);
                    usage = usage.Split("\n").First(x => x.Contains("Current")).Split(" ", StringSplitOptions.RemoveEmptyEntries).First(x => x.Contains("Rate"));
                    var value = (float)0;
                    if (usage.Contains(':'))
                    {
                        value = float.Parse(usage[(usage.IndexOf(':') + 1)..]) * 1048576;
                    }
                    else
                    {
                        value = float.Parse(usage[(usage.IndexOf('=') + 1)..]) * 1048576;
                    }
                    result.Add((adapter, value));
                }
                else
                {
                    var value = float.Parse(Regex.Replace(usage.Split(" ")[^1], "[^0-9]", "")) * 1048576;
                    result.Add((adapter, value));
                }
            }
            return result;
        }
        private static List<(string name, int size)> GetPhysicalDisks()
        {
            var result = new List<(string name, int size)>(); var command = new ProcessStartInfo("lsblk")
            {
                FileName = "/bin/bash",
                Arguments = "-c \"lsblk -dno NAME,SIZE\"",
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
            foreach (var line in commandOutput.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                var size = float.Parse(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1][..^1].Replace(',', '.'));
                result.Add((line.Split(" ")[0], Convert.ToInt32(size)));
            }
            return result;
        }
    }
}
