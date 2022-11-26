using HardwareMonitor.Infrastructure.DTO;
using SharedObjects;

namespace HardwareMonitor.Infrastructure.Services
{
    public interface ISystemInfoService
    {
        Task AddAsync(CreateSystemInfo createSystemInfo);
        Task DeleteAsync(List<string> id);
        Task<SystemInfoDTO> GetAsync(List<string> ids, int? limit);
        Task UpdateAsync(List<string> id);
        Task<IEnumerable<SystemInfoDTO>> GetAllAsync(int? limit);
    }
}
