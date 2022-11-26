using HardwareMonitor.DataSource.Reading;
using SharedObjects;
using System.Management;

namespace HardwareMonitor.DataSource
{
    public class SystemInfoWindows
    {
        public SystemReadingWindows SystemReadingWindows { get; set; }
        public SystemInfoWindows()
        {
            SystemReadingWindows = new SystemReadingWindows();
        }
        public CreateSystemInfo GetSystemInfo()
        {
            return new CreateSystemInfo()
            {
                SystemMacs = GetMacAddresses(),
                SystemName = Environment.MachineName,
                CreateSystemReadings = new List<CreateSystemReading> { SystemReadingWindows.GetSystemReading() }
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
