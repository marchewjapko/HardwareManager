using HardwareMonitor.Infrastructure.Commands;
using HardwareMonitor.Infrastructure.DTO;

namespace HardwareMonitor.Infrastructure.Services
{
    public interface IMachineService
    {
        Task<MachineDTO> AddAsync(CreateMachine createMachine);
        Task<MachineDTO> UpdateAsync(UpdateMachine updateMachine, string id);
        Task DeleteAsync(string id);
        Task<MachineDTO> GetAsync(string id);
        Task<IEnumerable<MachineDTO>> GetAllAsync();
    }
}
