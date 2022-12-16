using SharedObjects;
using SystemMonitor.Core.Repositories;
using SystemMonitor.Infrastructure.Commands;
using SystemMonitor.Infrastructure.DTO;
using SystemMonitor.Infrastructure.DTO.Conversions;

namespace SystemMonitor.Infrastructure.Services
{
    public class SystemInfoService : ISystemInfoService
    {
        private readonly ISystemInfoRepository _systemInfoRepository;
        private readonly ISystemReadingRepository _systemReadingRepository;
        public SystemInfoService(ISystemInfoRepository systemInfoRepository, ISystemReadingRepository systemReadingRepository)
        {
            _systemInfoRepository = systemInfoRepository;
            _systemReadingRepository = systemReadingRepository;
        }

        public async Task<Task> AddAsync(CreateSystemInfo createSystemInfo)
        {
            var system = await _systemInfoRepository.GetAsync(createSystemInfo.SystemMacs, 0);
            if (system == null)
            {
                await _systemInfoRepository.AddAsync(createSystemInfo.ToDomain());
                return Task.FromException(new Exception("system-created"));
            }
            else if (system.IsAuthorised)
            {
                await _systemReadingRepository.AddAsync(createSystemInfo.CreateSystemReadings.Select(x => x.ToDomain()).ToList(), system.Id);
                return Task.CompletedTask;
            }
            else
            {
                return Task.FromException(new Exception("system-not-authorized"));
            }
        }

        public async Task<Task> DeleteAsync(int id)
        {
            return await _systemInfoRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<SystemInfoDTO>> GetAllAsync(int? limit)
        {
            var result = await _systemInfoRepository.BrowseAllAsync(limit);
            return await Task.FromResult(result.Select(x => x.ToDTO()));
        }

        public async Task<SystemInfoDTO> GetAsync(int id, int? limit)
        {
            var result = await _systemInfoRepository.GetAsync(id, limit);
            if (result == null)
            {
                return null;
            }
            return result.ToDTO();
        }

        public async Task<Task> UpdateAsync(UpdateSystemInfo updateSystemInfo, int id)
        {
            return await _systemInfoRepository.UpdateAsync(updateSystemInfo.ToDomain(), id);
        }
    }
}
