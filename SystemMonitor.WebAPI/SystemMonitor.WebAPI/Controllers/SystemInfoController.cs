using SystemMonitor.Infrastructure.Commands;
using SystemMonitor.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using SharedObjects;

namespace SystemMonitor.WebAPI.Controllers
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
            var result = await _systemInfoService.AddAsync(createSystemInfo);
            if (result.Exception != null && result.Exception.InnerException.Message == "system-not-authorized")
            {
                return Unauthorized();
            }
            else if (result.Exception != null)
            {
                throw result.Exception.InnerException;
            }
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
        public async Task<IActionResult> GetSystem(int id, int? limit)
        {
            var result = await _systemInfoService.GetAsync(id, limit);
            if (result == null)
            {
                return NotFound();
            }
            return Json(result);
        }

        [Route("/DeleteSystem")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSystem([FromQuery] int id)
        {
            var result = await _systemInfoService.DeleteAsync(id);
            if (result.Exception != null && result.Exception.InnerException.Message == "not-found")
            {
                return NotFound();
            }
            else if (result.Exception != null)
            {
                throw result.Exception.InnerException;
            }
            return Ok();
        }

        [Route("/UpdateSystem")]
        [HttpPut]
        public async Task<IActionResult> UpdateSystem([FromBody] UpdateSystemInfo updateSystemInfo, int id)
        {
            var result = await _systemInfoService.UpdateAsync(updateSystemInfo, id);
            if (result.Exception != null && result.Exception.InnerException.Message == "not-found")
            {
                return NotFound();
            }
            else if (result.Exception != null)
            {
                throw result.Exception.InnerException;
            }
            return Ok();
        }
    }
}
