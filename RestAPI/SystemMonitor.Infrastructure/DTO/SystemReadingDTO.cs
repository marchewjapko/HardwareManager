namespace HardwareMonitor.Infrastructure.DTO
{
    public class SystemReadingDTO
    {
        public SystemUsageDTO UsageDTO { get; set; }
        public SystemSpecsDTO SystemSpecsDTO { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
