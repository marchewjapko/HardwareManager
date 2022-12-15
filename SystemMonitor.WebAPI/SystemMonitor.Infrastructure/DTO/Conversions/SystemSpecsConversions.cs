using SystemMonitor.Core.Domain;
using SharedObjects;
using SystemMonitor.Infrastructure.DTO.Conversions;

namespace SystemMonitor.Infrastructure.DTO.Conversions
{
    public static class SystemSpecsConversions
    {
        public static SystemSpecsDTO ToDTO(this SystemSpecs systemSpecs)
        {
            return new SystemSpecsDTO()
            {
                OsNameVersion = systemSpecs.OsNameVersion,
                CpuInfo = systemSpecs.CpuInfo,
                CpuCores = systemSpecs.CpuCores,
                TotalMemory = systemSpecs.TotalMemory,
                NetworkSpecs = systemSpecs.NetworkSpecs.Select(x => x.ToDTO()).ToList(),
                DiskSpecs = systemSpecs.DiskSpecs.Select(x => x.ToDTO()).ToList(),
            };

        }
        public static SystemSpecs ToDomain(this CreateSystemSpecs createSystemSpecs)
        {
            return new SystemSpecs()
            {
                OsNameVersion = createSystemSpecs.OsNameVersion,
                CpuInfo = createSystemSpecs.CpuInfo,
                CpuCores = createSystemSpecs.CpuCores,
                TotalMemory = createSystemSpecs.TotalMemory,
                NetworkSpecs = createSystemSpecs.CreateNetworkSpecs.Select(x => x.ToDomain()).ToList(),
                DiskSpecs = createSystemSpecs.CreateDiskSpecs.Select(x => x.ToDomain()).ToList(),
            };
        }
    }
}
