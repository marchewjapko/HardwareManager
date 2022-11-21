using DataSource.Specs;
using DataSource.Usage.Windows;
using SharedObjects;
using System.Management;

namespace HardwareMonitor.DataSource.Reading
{
    public class SystemReadingWindows
    {
        UsageMonitorWindows usageMonitorWindows { get; set; }
        SystemSpecsWindows systemSpecsWindows { get; set; }
        public SystemReadingWindows()
        {
            usageMonitorWindows = new UsageMonitorWindows();
            systemSpecsWindows = new SystemSpecsWindows();
        }

        public SystemInfoDTO GetSystemReading()
        {
            return new SystemInfoDTO()
            {
                SystemMacs = GetMacAddresses(),
                SystemName = Environment.MachineName,
                SystemSpecsDTO = new List<SystemSpecsDTO>()
                {
                    systemSpecsWindows.GetMachineSpecs()
                },
                UsageDTO = new List<UsageDTO>()
                {
                    usageMonitorWindows.GetSystemUsage()
                }
            };
        }

        private static List<string> GetMacAddresses()
        {
            var result = new List<string>();
            ManagementObjectSearcher adapterObjectSearcher = new("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter WHERE NetEnabled = 1");
            foreach (var adapter in adapterObjectSearcher.Get())
            {
                result.Add(adapter["MACAddress"].ToString());
            }
            return result;
        }
    }
}
