using DataSource.Usage.Windows.DataRetrieval;
using System.Runtime.Versioning;

namespace DataSource.Usage.Windows
{
    [SupportedOSPlatform("windows")]
    public class HardwareMonitorWindows
    {
        private CpuInfo cpuInfo;
        private DiskInfo diskInfo;
        private MemoryInfo memoryInfo;
        private NetworkInfo networkInfo;
        private SystemInfo systemInfo;

        public HardwareMonitorWindows()
        {
            cpuInfo = new CpuInfo();
            memoryInfo = new MemoryInfo();
            diskInfo = new DiskInfo();
            networkInfo = new NetworkInfo();
            systemInfo = new SystemInfo();
        }

        public UsageDTO GetSystemUsage()
        {
            diskInfo.UpdateDiskInstances();
            networkInfo.UpdateNetworkInstances();

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
