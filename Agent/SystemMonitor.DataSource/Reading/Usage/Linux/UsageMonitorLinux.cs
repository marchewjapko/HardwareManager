using DataSource.Usage.Linux.DataRetrieval;
using SharedObjects;
using System.Runtime.Versioning;

namespace DataSource.Usage.Linux
{
    [SupportedOSPlatform("linux")]
    public class UsageMonitorLinux
    {
        private CpuInfo cpuInfo;
        private DiskInfo diskInfo;
        private MemoryInfo memoryInfo;
        private NetworkInfo networkInfo;
        private SystemInfo systemInfo;

        public UsageMonitorLinux()
        {
            cpuInfo = new CpuInfo();
            memoryInfo = new MemoryInfo();
            diskInfo = new DiskInfo();
            networkInfo = new NetworkInfo();
            systemInfo = new SystemInfo();
        }

        public CreateUsage GetSystemUsage()
        {
            Parallel.Invoke(
                () => { cpuInfo.UpdateCpuReadingsLinux(); },
                () => { networkInfo.UpdateNetworkReadingsLinux(); },
                () => { diskInfo.UpdateDiskReadingsLinux(); }
            );

            return new CreateUsage()
            {
                CpuTotalUsage = cpuInfo.GetCpuTotalUsage(),
                CpuPerCoreUsage = cpuInfo.GetCpuPerCoreUsage(),
                DiskUsage = diskInfo.GetDiskUsage(),
                MemoryUsage = memoryInfo.GetRemainingMemory(),
                BytesReceived = networkInfo.GetBytesReceived(),
                BytesSent = networkInfo.GetBytesSent(),
                SystemUptime = systemInfo.GetSystemUptime(),
            };
        }
    }
}
