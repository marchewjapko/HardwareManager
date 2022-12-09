using DataSource.Helpers;
using SharedObjects;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using SystemMonitor.SharedObjects;

namespace DataSource.Specs
{
    [SupportedOSPlatform("linux")]
    public class SystemSpecsLinux
    {
        public CreateSystemSpecs GetMachineSpecs()
        {
            return new CreateSystemSpecs()
            {
                OsNameVersion = RuntimeInformation.OSDescription,
                CpuInfo = GetCpuInfo(),
                CpuCores = Environment.ProcessorCount,
                TotalMemory = GetTotalMemory(),
                CreateNetworkSpecs = GetNetworkAdapters(),
                CreateDiskSpecs = GetDisks(),
            };
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

        private static List<CreateNetworkSpecs> GetNetworkAdapters()
        {
            var result = new List<CreateNetworkSpecs>();
            var adapters = LinuxNetworkHelpers.GetAllNetworkAdapters();
            foreach (var adapter in adapters)
            {
                var usage = LinuxNetworkHelpers.GetEthernetBandwidth(adapter);
                if (usage == "")
                {
                    usage = LinuxNetworkHelpers.GetWirelessBandwidth(adapter);
                    usage = usage.Split("\n").First(x => x.Contains("Current")).Split(" ", StringSplitOptions.RemoveEmptyEntries).First(x => x.Contains("Rate"));
                    var value = 0.0;
                    if (usage.Contains(':'))
                    {
                        value = Convert.ToDouble(usage[(usage.IndexOf(':') + 1)..]) * 1048576;
                    }
                    else
                    {
                        value = Convert.ToDouble(usage[(usage.IndexOf('=') + 1)..]) * 1048576;
                    }
                    result.Add(new CreateNetworkSpecs()
                    {
                        AdapterName = adapter,
                        Bandwidth = value
                    });
                }
                else
                {
                    var value = Convert.ToDouble(Regex.Replace(usage.Split(" ")[^1], "[^0-9]", "")) * 1048576;
                    result.Add(new CreateNetworkSpecs()
                    {
                        AdapterName = adapter,
                        Bandwidth = value
                    });
                }
            }
            return result;
        }
        private static List<CreateDiskSpecs> GetDisks()
        {
            var result = new List<CreateDiskSpecs>(); var command = new ProcessStartInfo("lsblk")
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
                var size = Convert.ToDouble(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1][..^1].Replace(',', '.'));
                result.Add(new CreateDiskSpecs()
                {
                    DiskName = line.Split(" ")[0],
                    DiskSize = size
                });
            }
            return result;
        }
    }
}
