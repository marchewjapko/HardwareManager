using HardwareMonitor.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardwareMonitor.Core.Repositories
{
    public interface ISystemInfoRepository
    {
        Task AddAsync(SystemInfo machine);
        Task<SystemInfo> GetAsync(List<string> ids, int? limit);
        Task<SystemInfo> GetAsync(int id, int? limit);
        Task<Task> DeleteAsync(List<string> id);
        Task<Task> UpdateAsync(SystemInfo systemInfo, int id);
        Task<IEnumerable<SystemInfo>> BrowseAllAsync(int? limit);
    }
}
