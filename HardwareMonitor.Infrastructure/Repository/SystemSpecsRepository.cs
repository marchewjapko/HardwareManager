using HardwareMonitor.Core.Domain;
using HardwareMonitor.Core.Repositories;
using HardwareMonitor.RestAPI;

namespace HardwareMonitor.Infrastructure.Repository
{
    public class SystemSpecsRepository : ISystemSpecsRepository
    {
        private readonly AppDbContext _appDbContext;
        public SystemSpecsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddAsync(SystemSpecs systemSpecs, int id)
        {
            systemSpecs.SystemInfo = _appDbContext.SystemsInfos.FirstOrDefault(x => x.Id == id);
            _appDbContext.SystemSpecs.Add(systemSpecs);
            await _appDbContext.SaveChangesAsync();
            return;
        }

        public async Task AddAsync(List<SystemSpecs> systemsSpecs, int id)
        {
            var systemInfo = _appDbContext.SystemsInfos.FirstOrDefault(x => x.Id == id);
            foreach (var usage in systemsSpecs)
            {
                usage.SystemInfo = systemInfo;
            }
            _appDbContext.SystemSpecs.AddRange(systemsSpecs);
            await _appDbContext.SaveChangesAsync();
            return;
        }

        public async Task<IEnumerable<SystemSpecs>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.SystemSpecs);
        }

        public async Task DeleteAsync(int id)
        {
            var specs = await Task.FromResult(_appDbContext.SystemSpecs.FirstOrDefault(x => x.MachineId == id));
            if (specs == null)
            {
                throw new Exception("Unable to find specs");
            }
            _appDbContext.SystemSpecs.Remove(specs);
            _appDbContext.SaveChanges();
        }

        public async Task<SystemSpecs> GetAsync(int id)
        {
            var specs = await Task.FromResult(_appDbContext.SystemSpecs.FirstOrDefault(x => x.MachineId == id));
            if (specs == null)
            {
                throw new Exception("Unable to find specs");
            }
            return specs;
        }
    }
}
