using SystemMonitor.Core.Repositories;
using SystemMonitor.Infrastructure.DTO;
using SystemMonitor.Infrastructure.DTO.Conversions;

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

        public async Task<IEnumerable<SystemReadingDTO>> GetReadings(DateTime? from, DateTime? to, int systemId)
        {
            var result = await _systemReadingRepository.GetReadings(from, to, systemId);

            var lol = result.ToList();

            if(result == null)
            {
                return null;
            }
            return result.Select(x => x.ToDTO());
        }
    }
}
