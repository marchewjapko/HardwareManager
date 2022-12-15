using SharedObjects;
using SystemMonitor.Infrastructure.Commands;
using SystemMonitor.Infrastructure.DTO;

namespace SystemMonitor.Infrastructure.Services
{
    public interface ISystemInfoService
    {
        Task<Task> AddAsync(CreateSystemInfo createSystemInfo);
        Task<Task> DeleteAsync(int id);
        Task<SystemInfoDTO> GetAsync(int id, int? limit);
        Task<Task> UpdateAsync(UpdateSystemInfo updateSystemInfo, int id);
        Task<IEnumerable<SystemInfoDTO>> GetAllAsync(int? limit);
    }
}
