using HardwareMonitor.Core.Repositories;
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

        public async Task AddAsync(CreateSystemInfo createSystemInfo)
        {
            var system = await _systemInfoRepository.GetAsync(createSystemInfo.SystemMacs, 0);
            if (system == null)
            {
                await _systemInfoRepository.AddAsync(createSystemInfo.ToDomain());
                return;
            }
            else if (system.IsAuthorised)
            {
                var lol = createSystemInfo.CreateSystemReadings.ToList();
                await _systemReadingRepository.AddAsync(createSystemInfo.CreateSystemReadings.Select(x => x.ToDomain()).ToList(), system.Id);
                return;
            }
            else
            {
                throw new Exception("System is not authorized!");
            }
        }

        public async Task DeleteAsync(List<string> ids)
        {
            await _systemInfoRepository.DeleteAsync(ids);
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

        public async Task UpdateAsync(List<string> id)
        {
            throw new NotImplementedException();
        }
    }
}
