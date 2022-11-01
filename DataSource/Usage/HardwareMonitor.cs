using DataSource.Counters;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace DataSource.Usage
{
    [SupportedOSPlatform("windows"), SupportedOSPlatform("linux")]
    public class HardwareMonitor
    {
        private CpuInfo cpuInfo;
        private DiskInfo diskInfo;
        private MemoryInfo memoryInfo;
        private NetworkInfo networkInfo;
        private SystemInfo systemInfo;

        public HardwareMonitor()
        {
            cpuInfo = new CpuInfo();
            memoryInfo = new MemoryInfo();
            diskInfo = new DiskInfo();
            networkInfo = new NetworkInfo();
            systemInfo = new SystemInfo();
        }

        public UsageDTO GetSystemUsage()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Parallel.Invoke(
                    () => { cpuInfo.UpdateCpuReadingsLinux(); },
                    () => { networkInfo.UpdateNetworkReadingsLinux(); },
                    () => { diskInfo.UpdateDiskReadingsLinux(); }
                );
            }
            else
            {
                diskInfo.UpdateDiskInstances();
                networkInfo.UpdateNetworkInstances();
            }

            float totalUsage = 0;
            List<(string, float)> perCoreUsage = new();
            List<(string, float)> diskUsage = new();
            float memoryUsage = 0;
            List<(string, float)> bytesReceived = new();
            List<(string, float)> bytesSent = new();
            float systemUptime = 0;
            Parallel.Invoke(
               () => { totalUsage = cpuInfo.GetCpuTotalUsage(); },
               () => { perCoreUsage = cpuInfo.GetCpuPerCoreUsage(); },
               () => { diskUsage = diskInfo.GetDiskUsage(); },
               () => { memoryUsage = memoryInfo.GetRemainingMemory(); },
               () => { bytesReceived = networkInfo.GetBytesReceived(); },
               () => { bytesSent = networkInfo.GetBytesSent(); },
               () => { systemUptime = systemInfo.GetSystemUptime(); }
            );
            return new UsageDTO(
               totalUsage,
               perCoreUsage,
               diskUsage,
               memoryUsage,
               bytesReceived,
               bytesSent,
               systemUptime
            );

            // return new UsageDTO(
            //     cpuInfo.GetCpuTotalUsage(),
            //     cpuInfo.GetCpuPerCoreUsage(),
            //     diskInfo.GetDiskUsage(),
            //     memoryInfo.GetRemainingMemory(),
            //     networkInfo.GetBytesReceived(),
            //     networkInfo.GetBytesSent(),
            //     systemInfo.GetSystemUptime()
            // );
        }
    }
}
