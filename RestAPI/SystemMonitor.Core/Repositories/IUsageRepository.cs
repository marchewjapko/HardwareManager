using HardwareMonitor.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardwareMonitor.Core.Repositories
{
    public interface IUsageRepository
    {
        Task AddAsync(Usage usage, int id);
        Task AddAsync(List<Usage> usages, int id);
        Task<Usage> GetAsync(int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<Usage>> BrowseAllAsync();
    }
}
