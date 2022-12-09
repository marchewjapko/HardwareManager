using SharedObjects;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using SystemMonitor.SharedObjects;

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
                CreateNetworkSpecs = GetNetworkAdapters(),
                CreateDiskSpecs = GetDisks(),
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

        private static List<CreateNetworkSpecs> GetNetworkAdapters()
        {
            var result = new List<CreateNetworkSpecs>();
            var category = new PerformanceCounterCategory("Network Interface");
            string[] instances = category.GetInstanceNames();
            foreach (var instance in instances)
            {
                var bandwidth = new PerformanceCounter("Network Interface", "Current Bandwidth", instance);
                result.Add(new CreateNetworkSpecs()
                {
                    AdapterName = instance,
                    Bandwidth = bandwidth.NextValue()
                });
            }
            return result;
        }
        private static List<CreateDiskSpecs> GetDisks()
        {
            var result = new List<CreateDiskSpecs>();
            ManagementObjectSearcher diskObjectSearcher = new("root\\CIMV2", "SELECT * FROM Win32_LogicalDisk");
            foreach (var disk in diskObjectSearcher.Get())
            {
                result.Add(new CreateDiskSpecs()
                {
                    DiskName = disk["Name"].ToString(),
                    DiskSize = Convert.ToDouble(Convert.ToInt64(disk["Size"]))
                });
            }
            return result;
        }
    }
}
