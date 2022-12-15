namespace SystemMonitor.Core.Domain
{
    public class DiskUsage
    {
        public int Id { get; set; }
        public string DiskName { get; set; }
        public double Usage { get; set; }

        public int SystemUsageId { get; set; }
        public SystemUsage SystemUsage { get; set; }
    }
}
