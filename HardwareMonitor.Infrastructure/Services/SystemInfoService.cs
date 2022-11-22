using HardwareMonitor.Core.Repositories;
using HardwareMonitor.Infrastructure.Commands;
using HardwareMonitor.Infrastructure.DTO.Conversions;
using SharedObjects;

namespace HardwareMonitor.Infrastructure.Services
{
    public class SystemInfoService : ISystemInfoService
    {
        private readonly ISystemInfoRepository _systemInfoRepository;
        private readonly IUsageRepository _usageRepository;
        private readonly ISystemSpecsRepository _systemSpecsRepository;
        public SystemInfoService(ISystemInfoRepository systemInfoRepository, IUsageRepository usageRepository, ISystemSpecsRepository systemSpecsRepository)
        {
            _systemInfoRepository = systemInfoRepository;
            _usageRepository = usageRepository;
            _systemSpecsRepository = systemSpecsRepository;
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
                await _usageRepository.AddAsync(createSystemInfo.CreateUsage.Select(x => x.ToDomain()).ToList(), system.Id);
                await _systemSpecsRepository.AddAsync(createSystemInfo.SystemSpecs.Select(x => x.ToDomain()).ToList(), system.Id);
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
