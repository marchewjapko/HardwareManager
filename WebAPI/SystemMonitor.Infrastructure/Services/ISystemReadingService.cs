namespace SystemMonitor.Infrastructure.Services
{
    public interface ISystemReadingService
    {
        Task<Task> DeleteAsync(DateTime? from, DateTime? to, int systemId);
    }
}
