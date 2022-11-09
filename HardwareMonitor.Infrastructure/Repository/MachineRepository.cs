using HardwareMonitor.Core.Domain;
using HardwareMonitor.Core.Repositories;
using HardwareMonitor.RestAPI;

namespace HardwareMonitor.Infrastructure.Repository 
{
    public class MachineRepository : IMachineRepository
    {
        private readonly AppDbContext _appDbContext;
        public MachineRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Machine> AddAsync(Machine machine)
        {
            _appDbContext.Machines.Add(machine);
            await _appDbContext.SaveChangesAsync();
            return await Task.FromResult(machine);
        }

        public async Task<IEnumerable<Machine>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.Machines);
        }

        public async Task DeleteAsync(string id)
        {
            var machine = await Task.FromResult(_appDbContext.Machines.FirstOrDefault(x => x.MachineId == id));
            if(machine == null)
            {
                throw new Exception("Unable to find machine");
            }
            _appDbContext.Machines.Remove(machine);
            _appDbContext.SaveChanges();
        }

        public async Task<Machine> GetAsync(string id)
        {
            var machine = await Task.FromResult(_appDbContext.Machines.FirstOrDefault(x => x.MachineId == id));
            if (machine == null)
            {
                throw new Exception("Unable to find machine");
            }
            return machine;
        }

        public async Task<Machine> UpdateAsync(Machine machine, string id)
        {
            var m = await Task.FromResult(_appDbContext.Machines.FirstOrDefault(x => x.MachineId == id));
            if(m == null)
            {
                throw new Exception("Unable to find machine");
            }
            m.MachineName = machine.MachineName;
            m.IsAuthorised = machine.IsAuthorised;
            _appDbContext.SaveChanges();
            return m;
        }
    }
}
