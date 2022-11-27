using HardwareMonitor.Core.Domain;
using HardwareMonitor.Core.Repositories;
using HardwareMonitor.RestAPI;
using Microsoft.EntityFrameworkCore;

namespace HardwareMonitor.Infrastructure.Repository
{
    public class SystemInfoRepository : ISystemInfoRepository
    {
        private readonly AppDbContext _appDbContext;
        public SystemInfoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(SystemInfo systemInfo)
        {
            await Task.FromResult(_appDbContext.SystemsInfos.Add(systemInfo));
            await _appDbContext.SaveChangesAsync();
            return;
        }

        public async Task<IEnumerable<SystemInfo>> BrowseAllAsync(int? limit)
        {
            if (limit != null)
            {
                return await Task.FromResult(
                    _appDbContext.SystemsInfos
                    .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                        .ThenInclude(x => x.Usage)
                    .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                        .ThenInclude(x => x.SystemSpecs)
                );
            }
            return await Task.FromResult(_appDbContext.SystemsInfos
                .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                    .ThenInclude(x => x.Usage)
                .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                    .ThenInclude(x => x.SystemSpecs)
            );
        }

        public async Task<Task> DeleteAsync(List<string> ids)
        {
            var system = await Task.FromResult(
                _appDbContext.SystemsInfos.ToList()
                    .FirstOrDefault(x => x.SystemMacs.Split(";", StringSplitOptions.RemoveEmptyEntries)
                        .Any(a => ids.Contains(a))
                    )
                );
            if (system == null)
            {
                return Task.FromException(new Exception("not-found"));
            }
            await Task.FromResult(_appDbContext.SystemsInfos.Remove(system));
            return Task.FromResult(_appDbContext.SaveChanges());
        }

        public async Task<SystemInfo> GetAsync(List<string> ids, int? limit)
        {
            var system = await Task.FromResult(
                _appDbContext.SystemsInfos.ToList()
                    .FirstOrDefault(x => x.SystemMacs.Split(";", StringSplitOptions.RemoveEmptyEntries)
                        .Any(a => ids.Contains(a))
                    )
                );
            if (system == null)
            {
                return null;
            }
            if (limit != null)
            {
                return await Task.FromResult(
                    _appDbContext.SystemsInfos
                    .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                        .ThenInclude(x => x.Usage)
                    .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                        .ThenInclude(x => x.SystemSpecs)
                    .FirstOrDefault(x => x.Id == system.Id)
                );
            }
            else
            {
                return await Task.FromResult(
                    _appDbContext.SystemsInfos
                    .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                        .ThenInclude(x => x.Usage)
                    .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                        .ThenInclude(x => x.SystemSpecs)
                    .FirstOrDefault(x => x.Id == system.Id)
                );
            }
        }

        public async Task<Task> UpdateAsync(SystemInfo systemInfo, int id)
        {
            var system = await Task.FromResult(_appDbContext.SystemsInfos.FirstOrDefault(x => x.Id == id));
            if (system == null)
            {
                return Task.FromException(new Exception("not-found"));
            }
            system.SystemName = systemInfo.SystemName;
            system.IsAuthorised = systemInfo.IsAuthorised;
            system.SystemMacs = systemInfo.SystemMacs;
            return Task.FromResult(_appDbContext.SaveChanges());
        }
    }
}
