using SharedObjects;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace DataSource.Specs
{
    [SupportedOSPlatform("windows")]
    public class SystemSpecsWindows
    {
        public CreateSystemSpecs GetMachineSpecs()
        {
            return new CreateSystemSpecs()
            {
                OsNameVersion = RuntimeInformation.OSDescription,
                CpuInfo = GetCpuInfo(),
                CpuCores = Environment.ProcessorCount,
                TotalMemory = GetTotalMemory(),
                NetworkAdapters = GetNetworkAdapters(),
                Disks = GetPhysicalDisks(),
            };
        }

        private static string GetCpuInfo()
        {
            ManagementObjectSearcher cpuObjectSearcher = new("root\\CIMV2", "SELECT Name FROM Win32_Processor");
            return cpuObjectSearcher.Get().OfType<ManagementObject>().FirstOrDefault()["Name"].ToString();
        }

        private static double GetTotalMemory()
        {
            ManagementObjectSearcher memoryObjectSearcher = new("root\\CIMV2", "SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");
            return Convert.ToDouble(memoryObjectSearcher.Get().OfType<ManagementObject>().FirstOrDefault()["TotalVisibleMemorySize"]);
        }

        private static List<StringDoublePair> GetNetworkAdapters()
        {
            var result = new List<StringDoublePair>();
            var category = new PerformanceCounterCategory("Network Interface");
            string[] instances = category.GetInstanceNames();
            foreach (var instance in instances)
            {
                var bandwidth = new PerformanceCounter("Network Interface", "Current Bandwidth", instance);
                result.Add(new StringDoublePair()
                {
                    Item1 = instance,
                    Item2 = bandwidth.NextValue()
                });
            }
            return result;
        }
        private static List<StringDoublePair> GetPhysicalDisks()
        {
            var result = new List<StringDoublePair>();
            ManagementObjectSearcher diskObjectSearcher = new("root\\CIMV2", "SELECT * FROM Win32_LogicalDisk");
            foreach (var disk in diskObjectSearcher.Get())
            {
                result.Add(new StringDoublePair()
                {
                    Item1 = disk["Name"].ToString(),
                    Item2 = Convert.ToDouble(Convert.ToInt64(disk["Size"]))
                });
            }
            return result;
        }
    }
}
