using SystemMonitor.Core.Domain;
using SystemMonitor.SharedObjects;

namespace SystemMonitor.Infrastructure.DTO.Conversions
{
    public static class NetworkSpecsConversions
    {
        public static NetworkSpecsDTO ToDTO(this NetworkSpecs networkSpecs)
        {
            return new NetworkSpecsDTO()
            {
                AdapterName = networkSpecs.AdapterName,
                Bandwidth = networkSpecs.Bandwidth,
            };
        }
        public static NetworkSpecs ToDomain(this CreateNetworkSpecs createNetworkSpecs)
        {
            return new NetworkSpecs()
            {
                AdapterName = createNetworkSpecs.AdapterName,
                Bandwidth = createNetworkSpecs.Bandwidth,
            };
        }
    }
}
