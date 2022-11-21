using DataSource.Helpers;
using Newtonsoft.Json.Linq;
using SharedObjects;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace DataSource.Specs
{
    [SupportedOSPlatform("linux")]
    public class SystemSpecsLinux
    {
        public SystemSpecsDTO GetMachineSpecs()
        {
            return new SystemSpecsDTO(
                RuntimeInformation.OSDescription,
                GetCpuInfo(),
                Environment.ProcessorCount,
                GetTotalMemory(),
                GetNetworkAdapters(),
                GetPhysicalDisks(),
                DateTime.Now
            );
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

        private static List<StringDoublePair> GetNetworkAdapters()
        {
            var result = new List<StringDoublePair>();
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
                    result.Add(new StringDoublePair()
                    {
                        Item1 = adapter,
                        Item2 = value
                    });
                }
                else
                {
                    var value = Convert.ToDouble(Regex.Replace(usage.Split(" ")[^1], "[^0-9]", "")) * 1048576;
                    result.Add(new StringDoublePair()
                    {
                        Item1 = adapter,
                        Item2 = value
                    });
                }
            }
            return result;
        }
        private static List<StringDoublePair> GetPhysicalDisks()
        {
            var result = new List<StringDoublePair>(); var command = new ProcessStartInfo("lsblk")
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
                result.Add(new StringDoublePair()
                {
                    Item1 = line.Split(" ")[0],
                    Item2 = size
                });
            }
            return result;
        }
    }
}
