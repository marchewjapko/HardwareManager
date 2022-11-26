using HardwareMonitor.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using SharedObjects;

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

        [Route("/AddSystem")]
        [HttpPost]
        public async Task<IActionResult> AddSystem([FromBody] CreateSystemInfo createSystemInfo)
        {
            await _systemInfoService.AddAsync(createSystemInfo);
            return Ok();
        }

        [Route("/GetAllSystems")]
        [HttpGet]
        public async Task<IActionResult> BrowseAllSystems(int? limit)
        {
            var result = await _systemInfoService.GetAllAsync(limit);
            return Json(result);
        }

        [Route("/GetSystem")]
        [HttpGet]
        public async Task<IActionResult> GetSystemInfo([FromQuery] List<string> ids, int? limit)
        {
            var result = await _systemInfoService.GetAsync(ids, limit);
            if(result == null)
            {
                return NotFound();
            }
            return Json(result);
        }

        [Route("/DeleteSystem")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSystem([FromQuery] List<string> ids)
        {
            await _systemInfoService.DeleteAsync(ids);
            return Ok();
        }
    }
}
