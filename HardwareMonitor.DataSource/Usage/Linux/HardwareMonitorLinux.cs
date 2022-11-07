using DataSource.Usage.Linux.DataRetrieval;
using System.Runtime.Versioning;

namespace DataSource.Usage.Linux
{
    [SupportedOSPlatform("linux")]
    public class HardwareMonitorLinux
    {
        private CpuInfo cpuInfo;
        private DiskInfo diskInfo;
        private MemoryInfo memoryInfo;
        private NetworkInfo networkInfo;
        private SystemInfo systemInfo;

        public HardwareMonitorLinux()
        {
            cpuInfo = new CpuInfo();
            memoryInfo = new MemoryInfo();
            diskInfo = new DiskInfo();
            networkInfo = new NetworkInfo();
            systemInfo = new SystemInfo();
        }

        public UsageDTO GetSystemUsage()
        {
            Parallel.Invoke(
                () => { cpuInfo.UpdateCpuReadingsLinux(); },
                () => { networkInfo.UpdateNetworkReadingsLinux(); },
                () => { diskInfo.UpdateDiskReadingsLinux(); }
            );

            return new UsageDTO(
                cpuInfo.GetCpuTotalUsage(),
                cpuInfo.GetCpuPerCoreUsage(),
                diskInfo.GetDiskUsage(),
                memoryInfo.GetRemainingMemory(),
                networkInfo.GetBytesReceived(),
                networkInfo.GetBytesSent(),
                systemInfo.GetSystemUptime()
            );
        }
    }
}
