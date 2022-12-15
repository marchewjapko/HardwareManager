using SystemMonitor.Core.Repositories;

namespace SystemMonitor.Infrastructure.Services
{
    public class SystemReadingService : ISystemReadingService
    {
        private readonly ISystemReadingRepository _systemReadingRepository;
        public SystemReadingService(ISystemReadingRepository systemReadingRepository)
        {
            _systemReadingRepository = systemReadingRepository;
        }

        public async Task<Task> DeleteAsync(DateTime? from, DateTime? to, int systemId)
        {
            return await _systemReadingRepository.DeleteAsync(from, to, systemId);
        }
    }
}
