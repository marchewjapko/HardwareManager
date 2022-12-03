using DataSource.Helpers;
using HardwareMonitor.DataSource.Reading;
using SharedObjects;
using System.Diagnostics;

namespace HardwareMonitor.DataSource
{
    public class SystemInfoLinux
    {
        public SystemReadingLinux SystemReadingLinux { get; set; }
        public SystemInfoLinux()
        {
            SystemReadingLinux = new SystemReadingLinux();
        }
        public CreateSystemInfo GetSystemInfo()
        {
            return new CreateSystemInfo() {
                SystemMacs = GetMacAddresses(),
                SystemName = Environment.MachineName,
                CreateSystemReadings = new List<CreateSystemReading> { SystemReadingLinux.GetSystemReading() }
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
