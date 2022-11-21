using HardwareMonitor.Infrastructure.Commands;
using HardwareMonitor.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace HardwareMonitor.RestAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class SystemInfoController : Controller
    {
        private readonly ISystemInfoService _systemInfoService;
        public SystemInfoController(ISystemInfoService systemInfoService)
        {
            _systemInfoService = systemInfoService;
        }
        [HttpPost]
        public async Task<IActionResult> AddSystem([FromBody] CreateSystemInfo createSystemInfo)
        {
            await _systemInfoService.AddAsync(createSystemInfo);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> BrowseAllSystems()
        {
            var result = await _systemInfoService.GetAllAsync();
            return Json(result);
        }
    }
}
