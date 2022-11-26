namespace HardwareMonitor.Infrastructure.DTO
{
    public class SystemReadingDTO
    {
        public UsageDTO UsageDTO { get; set; }
        public SystemSpecsDTO SystemSpecsDTO { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
