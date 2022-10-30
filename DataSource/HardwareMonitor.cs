using DataSource.Counters;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace DataSource
{
    [SupportedOSPlatform("windows"), SupportedOSPlatform("linux")]
    public class HardwareMonitor
    {
        private CpuInfo cpuInfo;
        private DiskInfo diskInfo;
        private MemoryInfo memoryInfo;
        private NetworkInfo networkInfo;
        private SystemInfo systemInfo;

        public HardwareMonitor() {
            cpuInfo = new CpuInfo();
            memoryInfo = new MemoryInfo();
            diskInfo = new DiskInfo();
            networkInfo = new NetworkInfo();
            systemInfo = new SystemInfo();
        }

        public UsageDTO GetSystemUsage()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                cpuInfo.UpdateCpuReadingsLinux();
                networkInfo.UpdateNetworkReadingsLinux();
            }
            return new UsageDTO(
                cpuInfo.GetCpuTotalUsage(), 
                cpuInfo.GetCpuPerCoreUsage(),
                diskInfo.GetDiskUsage(),
                memoryInfo.GetRemainingMemory(),
                networkInfo.GetBandwidths(),
                networkInfo.GetBytesReceived(),
                networkInfo.GetBytesSent(),
                systemInfo.GetSystemUptime()
            );
        }
    }
}
