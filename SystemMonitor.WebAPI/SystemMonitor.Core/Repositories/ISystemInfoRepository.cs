using System.Collections.Generic;
using System.Threading.Tasks;
using SystemMonitor.Core.Domain;

namespace SystemMonitor.Core.Repositories
{
    public interface ISystemInfoRepository
    {
        Task AddAsync(SystemInfo machine);
        Task<SystemInfo> GetAsync(int id, int? limit);
        Task<SystemInfo> GetAsync(List<string> macs, int? limit);
        Task<Task> DeleteAsync(int id);
        Task<Task> UpdateAsync(SystemInfo systemInfo, int id);
        Task<IEnumerable<SystemInfo>> BrowseAllAsync(int? limit);
    }
}
