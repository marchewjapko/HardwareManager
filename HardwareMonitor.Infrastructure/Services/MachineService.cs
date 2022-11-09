using HardwareMonitor.Core.Repositories;
using HardwareMonitor.Infrastructure.Commands;
using HardwareMonitor.Infrastructure.DTO;
using HardwareMonitor.Infrastructure.DTO.Conversions;

namespace HardwareMonitor.Infrastructure.Services
{
    public class MachineService : IMachineService
    {
        private readonly IMachineRepository _machineRepository;
        public MachineService(IMachineRepository machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public async Task<MachineDTO> AddAsync(CreateMachine createMachine)
        {
            var result = await _machineRepository.AddAsync(createMachine.ToDomain());
            return result.ToDTO();
        }

        public async Task DeleteAsync(string id)
        {
            await _machineRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<MachineDTO>> GetAllAsync()
        {
            var result = await _machineRepository.BrowseAllAsync();
            return await Task.FromResult(result.Select(x => x.ToDTO()));
        }

        public async Task<MachineDTO> GetAsync(string id)
        {
            var result = await _machineRepository.GetAsync(id);
            return await Task.FromResult(result.ToDTO());
        }

        public async Task<MachineDTO> UpdateAsync(UpdateMachine updateMachine, string id)
        {
            var result = await _machineRepository.UpdateAsync(updateMachine.ToDomain(), id);
            return await Task.FromResult(result.ToDTO());
        }
    }
}
