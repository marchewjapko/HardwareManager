using DataSource.Helpers;
using DataSource.Specs;
using DataSource.Usage.Linux;
using DataSource.Usage.Windows;
using SharedObjects;
using System.Diagnostics;
using System.Management;

namespace HardwareMonitor.DataSource.Reading
{
    public class SystemReadingLinux
    {
        UsageMonitorLinux usageMonitorLinux { get; set; }
        SystemSpecsLinux systemSpecsLinux { get; set; }
        public SystemReadingLinux()
        {
            usageMonitorLinux = new UsageMonitorLinux();
            systemSpecsLinux = new SystemSpecsLinux();
        }

        public SystemInfoDTO GetSystemReading()
        {
            return new SystemInfoDTO()
            {
                SystemMacs = GetMacAddresses(),
                SystemName = Environment.MachineName,
                SystemSpecsDTO = new List<SystemSpecsDTO>()
                {
                    systemSpecsLinux.GetMachineSpecs()
                },
                UsageDTO = new List<UsageDTO>()
                {
                    usageMonitorLinux.GetSystemUsage()
                }
            };
        }

        private static List<string> GetMacAddresses()
        {
            var result = new List<string>();
            var adapters = LinuxNetworkHelpers.GetAllNetworkAdapters();
            foreach (var adapter in adapters)
            {
                var command = new ProcessStartInfo("cat")
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"cat /sys/class/net/" + adapter + "/address\"",
                    RedirectStandardOutput = true
                };
                using (var process = Process.Start(command))
                {
                    if (process == null)
                    {
                        throw new Exception("Error when executing process: " + command.Arguments);
                    }
                    result.Add(process.StandardOutput.ReadToEnd()[..^1]);
                }
            }
            return result;
        }
    }
}
