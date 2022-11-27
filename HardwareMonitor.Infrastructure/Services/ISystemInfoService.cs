using HardwareMonitor.Infrastructure.Commands;
using HardwareMonitor.Infrastructure.DTO;
using SharedObjects;

namespace HardwareMonitor.Infrastructure.Services
{
    public interface ISystemInfoService
    {
        Task<Task> AddAsync(CreateSystemInfo createSystemInfo);
        Task<Task> DeleteAsync(List<string> id);
        Task<SystemInfoDTO> GetAsync(List<string> ids, int? limit);
        Task<Task> UpdateAsync(UpdateSystemInfo updateSystemInfo, int id);
        Task<IEnumerable<SystemInfoDTO>> GetAllAsync(int? limit);
    }
}
