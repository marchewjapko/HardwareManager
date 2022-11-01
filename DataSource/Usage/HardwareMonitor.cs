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
                cpuInfo.UpdateCpuReadingsLinux();
                networkInfo.UpdateNetworkReadingsLinux();
            }
            else
            {
                diskInfo.UpdateDiskInstances();
                networkInfo.UpdateNetworkInstances();
            }

            //float totalUsage = 0;
            //List<(string, float)> perCoreUsage = new();
            //List<(string, float)> diskUsage = new();
            //float memoryUsage = 0;
            //List<(string, float)> bytesReceived = new();
            //List<(string, float)> bytesSent = new();
            //float systemUptime = 0;
            //Parallel.Invoke(
            //    () =>
            //    {
            //        totalUsage = cpuInfo.GetCpuTotalUsage();
            //        Console.WriteLine($"Method 1 Completed by Thread={Thread.CurrentThread.ManagedThreadId}");
            //    },
            //    () =>
            //    {
            //        perCoreUsage = cpuInfo.GetCpuPerCoreUsage();
            //        Console.WriteLine($"Method 2 Completed by Thread={Thread.CurrentThread.ManagedThreadId}");
            //    },
            //    () =>
            //    {
            //        diskUsage = diskInfo.GetDiskUsage();
            //        Console.WriteLine($"Method 3 Completed by Thread={Thread.CurrentThread.ManagedThreadId}");
            //    },
            //    () =>
            //    {
            //        memoryUsage = memoryInfo.GetRemainingMemory();
            //        Console.WriteLine($"Method 4 Completed by Thread={Thread.CurrentThread.ManagedThreadId}");
            //    },
            //    () =>
            //    {
            //        bytesReceived = networkInfo.GetBytesReceived();
            //        Console.WriteLine($"Method 5 Completed by Thread={Thread.CurrentThread.ManagedThreadId}");
            //    },
            //    () =>
            //    {
            //        bytesSent = networkInfo.GetBytesSent();
            //        Console.WriteLine($"Method 6 Completed by Thread={Thread.CurrentThread.ManagedThreadId}");
            //    },
            //    () =>
            //    {
            //        systemUptime = systemInfo.GetSystemUptime();
            //        Console.WriteLine($"Method 7 Completed by Thread={Thread.CurrentThread.ManagedThreadId}");
            //    }
            //);
            //return new UsageDTO(
            //    totalUsage,
            //    perCoreUsage,
            //    diskUsage,
            //    memoryUsage,
            //    bytesReceived,
            //    bytesSent,
            //    systemUptime
            //);

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
