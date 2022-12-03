namespace HardwareMonitor.Infrastructure.Services
{
    public interface ISystemReadingService
    {
        Task<Task> DeleteAsync(DateTime from, DateTime to, List<string> systemIds);
        Task<Task> DeleteAsync(int id);
        Task<Task> DeleteAsync(List<string> systemIds);
    }
}
