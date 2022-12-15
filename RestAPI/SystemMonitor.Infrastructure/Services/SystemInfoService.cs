using HardwareMonitor.Core.Repositories;
using HardwareMonitor.Infrastructure.Commands;
using HardwareMonitor.Infrastructure.DTO;
using HardwareMonitor.Infrastructure.DTO.Conversions;
using SharedObjects;

namespace HardwareMonitor.Infrastructure.Services
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
                return Task.FromException(new Exception("system-not-authorized"));
            }
            else if (system.IsAuthorised)
            {
                return await Task.FromResult(_systemReadingRepository.AddAsync(createSystemInfo.CreateSystemReadings.Select(x => x.ToDomain()).ToList(), system.Id));
            }
            else
            {
                return Task.FromException(new Exception("system-not-authorized"));
            }
        }

        public async Task<Task> DeleteAsync(List<string> ids)
        {
            return await _systemInfoRepository.DeleteAsync(ids);
        }

        public async Task<IEnumerable<SystemInfoDTO>> GetAllAsync(int? limit)
        {
            var result = await _systemInfoRepository.BrowseAllAsync(limit);
            return await Task.FromResult(result.Select(x => x.ToDTO()));
        }

        public async Task<SystemInfoDTO> GetAsync(List<string> ids, int? limit)
        {
            var result = await _systemInfoRepository.GetAsync(ids, limit);
            if(result == null)
            {
                return null;
            }
            return result.ToDTO();
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
