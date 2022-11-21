using HardwareMonitor.Core.Repositories;
using HardwareMonitor.Infrastructure.DTO.Conversions;
using SharedObjects;

namespace HardwareMonitor.Infrastructure.Services
{
    public class UsageService : IUsageService
    {
        private readonly IUsageRepository _usageRepository;
        public UsageService(IUsageRepository usageRepository)
        {
            _usageRepository = usageRepository;
        }
        public async Task<IEnumerable<UsageDTO>> GetAllAsync()
        {
            var result = await _usageRepository.BrowseAllAsync();
            return await Task.FromResult(result.Select(x => x.ToDTO()));
        }
    }
}
