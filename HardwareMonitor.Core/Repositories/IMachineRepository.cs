using HardwareMonitor.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardwareMonitor.Core.Repositories
{
    public interface IMachineRepository
    {
        Task<Machine> AddAsync(Machine machine);
        Task<Machine> GetAsync(string id);
        Task DeleteAsync(string id);
        Task<Machine> UpdateAsync(Machine machine, string id);
        Task<IEnumerable<Machine>> BrowseAllAsync();
    }
}
