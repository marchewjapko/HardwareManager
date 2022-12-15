using SystemMonitor.Core.Domain;
using SystemMonitor.Infrastructure.Commands;
using SharedObjects;

namespace SystemMonitor.Infrastructure.DTO.Conversions
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

        public static SystemInfo ToDomain(this UpdateSystemInfo updateSystemInfo)
        {
            return new SystemInfo()
            {
                IsAuthorised = updateSystemInfo.IsAuthorised,
                SystemMacs = string.Join(";", updateSystemInfo.SystemMacs),
                SystemName = updateSystemInfo.SystemName
            };
        }
    }
}
