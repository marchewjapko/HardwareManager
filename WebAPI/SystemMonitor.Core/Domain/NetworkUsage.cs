namespace SystemMonitor.Core.Domain
{
    public class NetworkUsage
    {
        public int Id { get; set; }
        public string AdapterName { get; set; }
        public double BytesSent { get; set; }
        public double BytesReceived { get; set; }

        public int SystemUsageId { get; set; }
        public SystemUsage SystemUsage { get; set; }
    }
}
