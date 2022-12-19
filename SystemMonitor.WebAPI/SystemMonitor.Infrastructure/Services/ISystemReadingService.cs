using SystemMonitor.Infrastructure.DTO;

namespace SystemMonitor.Infrastructure.Services
{
    public interface ISystemReadingService
    {
        Task<Task> DeleteAsync(DateTime? from, DateTime? to, int systemId);
        Task<IEnumerable<SystemReadingDTO>> GetReadings(DateTime? from, DateTime? to, int systemId);
    }
}
