using DataSource.Counters;
using System.Runtime.Versioning;

namespace DataSource
{
    [SupportedOSPlatform("windows")]
    public class HardwareMonitor
    {
        private CpuInfo cpuInfo;
        private DiskInfo diskInfo;
        private MemoryInfo memoryInfo;
        private NetworkInfo networkInfo;
        private SystemInfo systemInfo;
        public HardwareMonitor() {
            cpuInfo = new CpuInfo();
            diskInfo = new DiskInfo();
            memoryInfo = new MemoryInfo();
            networkInfo = new NetworkInfo();
            systemInfo = new SystemInfo();
        }
        public UsageDTO GetSystemUsage()
        {
            return new UsageDTO(
                cpuInfo.GetCpuTotalUsage(), 
                cpuInfo.GetCpuPerCoreUsage(), 
                diskInfo.GetDiskUsage(), 
                memoryInfo.GetRemainingMemory(), 
                networkInfo.GetBandwidths(), 
                networkInfo.GetBytesReceived(), 
                networkInfo.GetBytesSent(),
                systemInfo.GetSystemUptime(), 
                systemInfo.GetSystemCalls()
            );
        }
    }
}
