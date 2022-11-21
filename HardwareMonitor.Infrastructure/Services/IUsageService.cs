using SharedObjects;

namespace HardwareMonitor.Infrastructure.Services
{
    public interface IUsageService
    {
        Task<IEnumerable<UsageDTO>> GetAllAsync();
    }
}
