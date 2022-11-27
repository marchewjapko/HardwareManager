using HardwareMonitor.Core.Repositories;

namespace HardwareMonitor.Infrastructure.Services
{
    public class SystemReadingService : ISystemReadingService
    {
        private readonly ISystemReadingRepository _systemReadingRepository;
        public SystemReadingService(ISystemReadingRepository systemReadingRepository)
        {
            _systemReadingRepository = systemReadingRepository;
        }

        public async Task<Task> DeleteAsync(int id)
        {
            return await _systemReadingRepository.DeleteAsync(id);
        }

        public async Task<Task> DeleteAsync(List<string> systemIds)
        {
            return await _systemReadingRepository.DeleteAsync(systemIds);
        }

        public async Task<Task> DeleteAsync(DateTime from, DateTime to, List<string> systemIds)
        {
            return await _systemReadingRepository.DeleteAsync(from, to, systemIds);
        }
    }
}
