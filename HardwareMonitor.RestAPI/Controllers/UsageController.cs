using HardwareMonitor.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace HardwareMonitor.RestAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UsageController : Controller
    {
        private readonly IUsageService _usageService;
        public UsageController(IUsageService usageService)
        {
            _usageService = usageService;
        }

        [HttpGet]
        public async Task<IActionResult> BrowseAllSystems()
        {
            var result = await _usageService.GetAllAsync();
            return Json(result);
        }
    }
}
