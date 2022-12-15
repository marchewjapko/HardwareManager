using SystemMonitor.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace SystemMonitor.WebAPI.Controllers
{
    public class SystemReadingController : Controller
    {
        private readonly ISystemReadingService _systemReadingService;
        public SystemReadingController(ISystemReadingService systemReadingService)
        {
            _systemReadingService = systemReadingService;
        }
        [Route("/Delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSystem(DateTime? from, DateTime? to, int systemId)
        {
            var result = await _systemReadingService.DeleteAsync(from, to, systemId);
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
