using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace DataSource.Specs
{
    [SupportedOSPlatform("windows")]
    public class MachineSpecsWindows
    {
        private string machineName;
        private string osNameVersion;
        private string cpuInfo;
        private int cpuCores;
        private double totalMemory;
        private List<(string name, float bandwidth)> networkAdapters;
        private List<(string name, int size)> physicalDisks;
        private List<string> macAddresses;

        public MachineSpecsWindows GetMachineSpecs()
        {
            machineName = Environment.MachineName;
            osNameVersion = RuntimeInformation.OSDescription;
            cpuCores = Environment.ProcessorCount;
            cpuInfo = GetCpuInfo();
            totalMemory = GetTotalMemory();
            networkAdapters = GetNetworkAdapters();
            physicalDisks = GetPhysicalDisks();
            macAddresses = GetMacAddresses();
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
            result.Append("MAC addresses: " + "\n");
            foreach (var mac in macAddresses)
            {
                result.Append("\t " + mac + "\n");
            }
            return result.ToString();
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

        private static List<(string name, float bandwidth)> GetNetworkAdapters()
        {
            var result = new List<(string, float)>();
            var category = new PerformanceCounterCategory("Network Interface");
            string[] instances = category.GetInstanceNames();
            foreach (var instance in instances)
            {
                var bandwidth = new PerformanceCounter("Network Interface", "Current Bandwidth", instance);
                result.Add((instance, bandwidth.NextValue()));
            }
            return result;
        }
        private static List<(string name, int size)> GetPhysicalDisks()
        {
            var result = new List<(string name, int size)>();
            ManagementObjectSearcher diskObjectSearcher = new("root\\CIMV2", "SELECT * FROM Win32_LogicalDisk");
            foreach (var disk in diskObjectSearcher.Get())
            {
                result.Add((disk["Name"].ToString(), Convert.ToInt32(Convert.ToInt64(disk["Size"]) / 1073741824)));
            }
            return result;
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
