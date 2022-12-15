namespace SystemMonitor.Infrastructure.DTO
{
    public class NetworkUsageDTO
    {
        public string AdapterName { get; set; }
        public double BytesSent { get; set; }
        public double BytesReceived { get; set; }
    }
}
