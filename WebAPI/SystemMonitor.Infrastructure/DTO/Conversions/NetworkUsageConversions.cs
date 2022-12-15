using SystemMonitor.Core.Domain;
using SystemMonitor.SharedObjects;

namespace SystemMonitor.Infrastructure.DTO.Conversions
{
    public static class NetworkUsageConversions
    {
        public static NetworkUsageDTO ToDTO(this NetworkUsage networkUsage)
        {
            return new NetworkUsageDTO()
            {
                AdapterName = networkUsage.AdapterName,
                BytesSent = networkUsage.BytesSent,
                BytesReceived = networkUsage.BytesReceived,
            };
        }
        public static NetworkUsage ToDomain(this CreateNetworkUsage createNetworkUsage)
        {
            return new NetworkUsage()
            {
                AdapterName = createNetworkUsage.AdapterName,
                BytesSent = createNetworkUsage.BytesSent,
                BytesReceived = createNetworkUsage.BytesReceived,
            };
        }
    }
}
