using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using SystemMonitor.Infrastructure.Services;

namespace SystemMonitor.WebAPI.Hubs
{
    [SignalRHub]
    public class SystemReadingHub : Hub
    {
        private readonly ISystemReadingService _systemReadingService;
        public SystemReadingHub(ISystemReadingService systemReadingService)
        {
            _systemReadingService = systemReadingService;
        }

        public async Task<string> GetReadings(DateTime? from, DateTime? to, int systemId)
        {
            var result = await _systemReadingService.GetReadings(from, to, systemId);
            await Clients.Caller.SendAsync("ReceiveReadings", result.ToList());
            return "ok";
        }
    }
}
