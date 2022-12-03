using HardwareMonitor.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace HardwareMonitor.RestAPI.Controllers
{
    public class SystemReadingController : Controller
    {
        private readonly ISystemReadingService _systemReadingService;
        public SystemReadingController(ISystemReadingService systemReadingService)
        {
            _systemReadingService = systemReadingService;
        }
        [Route("/DeleteById")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSystem(int id)
        {
            var result = await _systemReadingService.DeleteAsync(id);
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
        [Route("/DeleteBySystem")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSystem([FromQuery] List<string> ids)
        {
            var result = await _systemReadingService.DeleteAsync(ids);
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
        [Route("/DeleteByDateRange")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSystem(DateTime from, DateTime to, List<string> systemIds)
        {
            var result = await _systemReadingService.DeleteAsync(from, to, systemIds);
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
