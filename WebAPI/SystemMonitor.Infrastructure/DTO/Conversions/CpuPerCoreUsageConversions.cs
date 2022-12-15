using SystemMonitor.Core.Domain;
using SystemMonitor.SharedObjects;

namespace SystemMonitor.Infrastructure.DTO.Conversions
{
    public static class CpuPerCoreUsageConversions
    {
        public static CpuPerCoreUsageDTO ToDTO(this CpuPerCoreUsage cpuPerCoreUsage)
        {
            return new CpuPerCoreUsageDTO()
            {
                Instance = cpuPerCoreUsage.Instance,
                Usage = cpuPerCoreUsage.Usage,
            };
        }
        public static CpuPerCoreUsage ToDomain(this CreateCpuPerCoreUsage createCpuPerCoreUsage)
        {
            return new CpuPerCoreUsage()
            {
                Instance = createCpuPerCoreUsage.Instance,
                Usage = createCpuPerCoreUsage.Usage,
            };
        }
    }
}
