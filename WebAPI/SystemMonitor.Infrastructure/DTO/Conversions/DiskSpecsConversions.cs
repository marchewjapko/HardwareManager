using SystemMonitor.Core.Domain;
using SystemMonitor.SharedObjects;

namespace SystemMonitor.Infrastructure.DTO.Conversions
{
    public static class DiskSpecsConversions
    {
        public static DiskSpecsDTO ToDTO(this DiskSpecs diskSpecs)
        {
            return new DiskSpecsDTO()
            {
                DiskName = diskSpecs.DiskName,
                DiskSize = diskSpecs.DiskSize,
            };
        }
        public static DiskSpecs ToDomain(this CreateDiskSpecs createDiskSpecs)
        {
            return new DiskSpecs()
            {
                DiskName = createDiskSpecs.DiskName,
                DiskSize = createDiskSpecs.DiskSize,
            };
        }
    }
}
