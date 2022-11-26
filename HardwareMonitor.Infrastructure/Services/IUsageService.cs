using HardwareMonitor.Infrastructure.DTO;

namespace HardwareMonitor.Infrastructure.Services
{
    public interface IUsageService
    {
        Task<IEnumerable<UsageDTO>> GetAllAsync();
    }
}
