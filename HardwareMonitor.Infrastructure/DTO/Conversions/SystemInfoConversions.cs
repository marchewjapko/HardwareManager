using HardwareMonitor.Core.Domain;
using HardwareMonitor.Infrastructure.Commands;
using SharedObjects;
using System.Linq;

namespace HardwareMonitor.Infrastructure.DTO.Conversions
{
    public static class SystemInfoConversions
    {
        public static SystemInfo ToDomain(this CreateSystemInfo createSystemInfo)
        {
            return new SystemInfo()
            {
                SystemMacs = String.Join(';', createSystemInfo.SystemMacs),
                SystemName = createSystemInfo.SystemName
            };
        }

        public static SystemInfoDTO ToDTO(this SystemInfo systemInfo)
        {
            return new SystemInfoDTO()
            {
                Id = systemInfo.Id,
                IsAuthorised = systemInfo.IsAuthorised,
                SystemMacs = systemInfo.SystemMacs.Split(";").ToList(),
                SystemName = systemInfo.SystemName,
                UsageDTO = systemInfo.Usages.Select(x => x.ToDTO()).ToList(),
                SystemSpecsDTO = systemInfo.SystemsSpecs.Select(x => x.ToDTO()).ToList()
            };
        }
    }
}
