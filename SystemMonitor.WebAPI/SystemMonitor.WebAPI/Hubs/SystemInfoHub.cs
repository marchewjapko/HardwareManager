using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SharedObjects;
using SignalRSwaggerGen.Attributes;
using System.Collections.Generic;
using System.Diagnostics;
using SystemMonitor.Infrastructure.Commands;
using SystemMonitor.Infrastructure.Services;

namespace SystemMonitor.WebAPI.Hubs
{
    [SignalRHub]
    public class SystemInfoHub : Hub
    {
        private readonly ISystemInfoService _systemInfoService;
        private readonly ISystemReadingService _systemReadingService;
        public SystemInfoHub(ISystemInfoService systemInfoService, ISystemReadingService systemReadingService)
        {
            _systemInfoService = systemInfoService;
            _systemReadingService = systemReadingService;
        }

        public async Task<string> AddSystem(CreateSystemInfo createSystemInfo)
        {
            var result = await _systemInfoService.AddAsync(createSystemInfo);
            if (result.Exception != null && result.Exception.InnerException.Message == "system-not-authorized")
            {
                return "unauthorised";
            }
            else if (result.Exception != null && result.Exception.InnerException.Message == "system-created")
            {
                await ForceClientsUpdate();
                return "system created";
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
            await ForceClientsUpdate();
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
            await ForceClientsUpdate();
            return "ok";
        }

        public async Task<string> BrowseAllSystems(int? limit)
        {
            var result = await _systemInfoService.GetAllAsync(limit);
            await Clients.Caller.SendAsync("ReceiveAllSystems", result.ToList());
            return "ok";
        }

        public async Task<string> GetSystem(int id, int? limit)
        {
            var result = await _systemInfoService.GetAsync(id, limit);
            await Clients.Caller.SendAsync("ReceiveSystem", result);
            if (result == null)
            {
                return "not-found";
            }
            return "ok";
        }

        public async Task<string> GetReadings(DateTime? from, DateTime? to, int systemId)
        {
            var result = await _systemReadingService.GetReadings(from, to, systemId);
            await Clients.Caller.SendAsync("ReceiveReadings", result.ToList());
            return "ok";
        }

        public async Task<string> GetGroupedReadings(DateTime? from, DateTime? to, int systemId)
        {
            var result = await _systemReadingService.GetReadings(from, to, systemId);
            await Clients.Caller.SendAsync("ReceiveGroupedReadings", result.ToList());
            return "ok";
        }

        public async Task ForceClientsUpdate()
        {
            var result = await _systemInfoService.GetAllAsync(1);
            await Clients.All.SendAsync("ReceiveAllSystems", result.ToList());
            return;
        }
    }
}
