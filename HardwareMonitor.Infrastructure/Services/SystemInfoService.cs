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
            var system = await _systemInfoRepository.GetAsync(createSystemInfo.SystemMacs);
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

        public Task DeleteAsync(List<string> id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SystemInfoDTO>> GetAllAsync()
        {
            var result = await _systemInfoRepository.BrowseAllAsync();
            return await Task.FromResult(result.Select(x => x.ToDTO()));
        }

        public Task<SystemInfoDTO> GetAsync(List<string> id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(List<string> id)
        {
            throw new NotImplementedException();
        }
    }
}
