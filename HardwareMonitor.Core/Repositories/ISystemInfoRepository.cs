using HardwareMonitor.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardwareMonitor.Core.Repositories
{
    public interface ISystemInfoRepository
    {
        Task AddAsync(SystemInfo machine);
        Task<SystemInfo> GetAsync(List<string> ids, int? limit);
        Task DeleteAsync(List<string> id);
        Task UpdateAsync(SystemInfo systemInfo, List<string> ids);
        Task<IEnumerable<SystemInfo>> BrowseAllAsync(int? limit);
    }
}
