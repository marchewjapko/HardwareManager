using SystemMonitor.Infrastructure.Commands;
using SystemMonitor.Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;
using SharedObjects;
using SignalRSwaggerGen.Attributes;

namespace SystemMonitor.WebAPI.Hubs
{
    [SignalRHub]
    public class SystemInfoHub : Hub
    {
        private readonly ISystemInfoService _systemInfoService;
        public SystemInfoHub(ISystemInfoService systemInfoService)
        {
            _systemInfoService = systemInfoService;
        }

        public async Task<string> AddSystem(CreateSystemInfo createSystemInfo)
        {
            var result = await _systemInfoService.AddAsync(createSystemInfo);
            if (result.Exception != null && result.Exception.InnerException.Message == "system-not-authorized")
            {
                return "unauthorised";
            }
            else if (result.Exception != null)
            {
                throw result.Exception.InnerException;
            }
            return "ok";
        }

        public async Task<string> DeleteSystem(int id)
        {
            var result = await _systemInfoService.DeleteAsync(id);
            if (result.Exception != null && result.Exception.InnerException.Message == "not-found")
            {
                return "not-found";
            }
            else if (result.Exception != null)
            {
                throw result.Exception.InnerException;
            }
            return "ok";
        }

        public async Task<string> UpdateSystem(UpdateSystemInfo updateSystemInfo, int id)
        {
            var result = await _systemInfoService.UpdateAsync(updateSystemInfo, id);
            if (result.Exception != null && result.Exception.InnerException.Message == "not-found")
            {
                return "not-found";
            }
            else if (result.Exception != null)
            {
                throw result.Exception.InnerException;
            }
            return "ok";
        }
    }
}
