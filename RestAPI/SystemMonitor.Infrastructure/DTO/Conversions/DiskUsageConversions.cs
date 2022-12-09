using SystemMonitor.Core.Domain;
using SystemMonitor.SharedObjects;

namespace SystemMonitor.Infrastructure.DTO.Conversions
{
    public static class DiskUsageConversions
    {
        public static DiskUsageDTO ToDTO(this DiskUsage diskUsage)
        {
            return new DiskUsageDTO()
            {
                DiskName = diskUsage.DiskName,
                Usage = diskUsage.Usage,
            };
        }
        public static DiskUsage ToDomain(this CreateDiskUsage createDiskUsage)
        {
            return new DiskUsage()
            {
                DiskName = createDiskUsage.DiskName,
                Usage = createDiskUsage.Usage,
            };
        }
    }
}
