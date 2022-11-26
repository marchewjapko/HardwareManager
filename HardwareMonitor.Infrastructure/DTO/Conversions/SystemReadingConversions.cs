using HardwareMonitor.Core.Domain;
using SharedObjects;

namespace HardwareMonitor.Infrastructure.DTO.Conversions
{
    public static class SystemReadingConversions
    {
        public static SystemReading ToDomain(this CreateSystemReading createSystemReading)
        {
            return new SystemReading()
            {
                Usage = createSystemReading.CreateUsage.ToDomain(),
                SystemSpecs = createSystemReading.CreateSystemSpecs.ToDomain(),
                Timestamp = createSystemReading.Timestamp
            };
        }

        public static SystemReadingDTO ToDTO(this SystemReading systemReading)
        {
            return new SystemReadingDTO()
            {
                UsageDTO = systemReading.Usage.ToDTO(),
                SystemSpecsDTO = systemReading.SystemSpecs.ToDTO(),
                Timestamp = systemReading.Timestamp
            };
        }
    }
}
