using HardwareMonitor.Core.Domain;
using HardwareMonitor.Core.Repositories;
using HardwareMonitor.RestAPI;

namespace HardwareMonitor.Infrastructure.Repository
{
    public class UsageRepository : IUsageRepository
    {
        private readonly AppDbContext _appDbContext;
        public UsageRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Usage usage, int id)
        {
            //usage.SystemInfo = _appDbContext.SystemsInfos.FirstOrDefault(x => x.Id == id);
            //_appDbContext.Usages.Add(usage);
            //await _appDbContext.SaveChangesAsync();
            //return;
            throw new NotImplementedException();
        }

        public async Task AddAsync(List<Usage> usages, int id)
        {
            //var systemInfo = _appDbContext.SystemsInfos.FirstOrDefault(x => x.Id == id);
            //foreach(var usage in usages)
            //{
            //    usage.SystemInfo = systemInfo;
            //}
            //_appDbContext.Usages.AddRange(usages);
            //await _appDbContext.SaveChangesAsync();
            //return;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Usage>> BrowseAllAsync()
        {
            return await Task.FromResult(_appDbContext.Usages);
        }

        public async Task DeleteAsync(int id)
        {
            //var usage = await Task.FromResult(_appDbContext.Usages.FirstOrDefault(x => x.MachineId == id));
            //if (usage == null)
            //{
            //    throw new Exception("Unable to find usage");
            //}
            //_appDbContext.Usages.Remove(usage);
            //_appDbContext.SaveChanges();
            throw new NotImplementedException();
        }

        public async Task<Usage> GetAsync(int id)
        {
            //var usage = await Task.FromResult(_appDbContext.Usages.FirstOrDefault(x => x.MachineId == id));
            //if (usage == null)
            //{
            //    throw new Exception("Unable to find usage");
            //}
            //return usage;
            throw new NotImplementedException();
        }
    }
}
