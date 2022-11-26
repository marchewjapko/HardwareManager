using HardwareMonitor.Core.Domain;
using SharedObjects;

namespace HardwareMonitor.Infrastructure.DTO.Conversions
{
    public static class SystemInfoConversions
    {
        public static SystemInfo ToDomain(this CreateSystemInfo createSystemInfo)
        {
            return new SystemInfo()
            {
                SystemMacs = string.Join(';', createSystemInfo.SystemMacs),
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
                SystemReadingDTOs = systemInfo.SystemReadings.Select(x => x.ToDTO()).ToList(),
            };
        }
    }
}
