using Microsoft.AspNetCore.Mvc;

namespace HardwareMonitor.RestAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class MachineController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public MachineController (AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> BrowseAllClients()
        {
            var result = await Task.FromResult(_appDbContext.Machines);
            return Json(result);
        }
    }
}
