using HardwareMonitor.Infrastructure.Commands;
using HardwareMonitor.Infrastructure.DTO;
using HardwareMonitor.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace HardwareMonitor.RestAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class MachineController : Controller
    {
        private readonly IMachineService _machineService;
        public MachineController (IMachineService machineService)
        {
            _machineService = machineService;
        }
        [HttpPost]
        public async Task<IActionResult> AddMachine([FromBody] CreateMachine createMachine)
        {
            var result = await _machineService.AddAsync(createMachine);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> BrowseAllClients()
        {
            var result = await _machineService.GetAllAsync();
            return Json(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(string id)
        {
            await _machineService.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(string id)
        {
            MachineDTO clientDTO = await _machineService.GetAsync(id);
            return Json(clientDTO);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateMachine updateClient, string id)
        {
            var result = await _machineService.UpdateAsync(updateClient, id);
            return Json(result);
        }
    }
}
