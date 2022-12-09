using HardwareMonitor.Core.Domain;
using SharedObjects;
using SystemMonitor.Infrastructure.DTO.Conversions;

namespace HardwareMonitor.Infrastructure.DTO.Conversions
{
    public static class SystemUsageConversions
    {
        public static SystemUsageDTO ToDTO(this SystemUsage usage)
        {
            return new SystemUsageDTO()
            {
                CpuTotalUsage = usage.CpuTotalUsage,
                CpuPerCoreUsage = usage.CpuPerCoreUsage.Select(x => x.ToDTO()).ToList(),
                DiskUsage = usage.DiskUsage.Select(x => x.ToDTO()).ToList(),
                MemoryUsage = usage.MemoryUsage,
                NetworkUsage = usage.NetworkUsage.Select(x => x.ToDTO()).ToList(),
                SystemUptime = usage.SystemUptime,
            };
        }
        public static SystemUsage ToDomain(this CreateSystemUsage createUsage)
        {
            return new SystemUsage()
            {
                CpuTotalUsage = createUsage.CpuTotalUsage,
                CpuPerCoreUsage = createUsage.CreateCpuPerCoreUsage.Select(x => x.ToDomain()).ToList(),
                DiskUsage = createUsage.CreateDiskUsage.Select(x => x.ToDomain()).ToList(),
                MemoryUsage = createUsage.MemoryUsage,
                NetworkUsage = createUsage.CreateNetworkUsage.Select(x => x.ToDomain()).ToList(),
                SystemUptime = createUsage.SystemUptime,
            };
        }
    }
}
