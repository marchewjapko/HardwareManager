using HardwareMonitor.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardwareMonitor.Core.Repositories
{
    public interface ISystemSpecsRepository
    {
        Task AddAsync(SystemSpecs systemSpecs, int id);
        Task AddAsync(List<SystemSpecs> systemsSpecs, int id);
        Task<SystemSpecs> GetAsync(int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<SystemSpecs>> BrowseAllAsync();
    }
}
