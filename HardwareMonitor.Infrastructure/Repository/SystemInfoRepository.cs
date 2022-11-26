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
            _appDbContext.SystemsInfos.Add(systemInfo);
            await _appDbContext.SaveChangesAsync();
            return;
        }

        public async Task<IEnumerable<SystemInfo>> BrowseAllAsync(int? limit)
        {
            if(limit != null)
            {
                return await Task.FromResult(
                    _appDbContext.SystemsInfos
                    .Include(x => x.SystemReadings.Take(Convert.ToInt32(limit)))
                    .ThenInclude(x => x.Usage)
                    .Include(x => x.SystemReadings.Take(Convert.ToInt32(limit)))
                    .ThenInclude(x => x.SystemSpecs)
                );
            }
            return await Task.FromResult(_appDbContext.SystemsInfos.Include(x => x.SystemReadings));
        }

        public async Task DeleteAsync(List<string> ids)
        {
            var system = await Task.FromResult(
                _appDbContext.SystemsInfos.FirstOrDefault(
                    x => x.SystemMacs.Split(";", StringSplitOptions.RemoveEmptyEntries).Intersect(ids).Count() > 0
                )
            );
            if (system == null)
            {
                throw new Exception("Unable to find systemInfo");
            }
            _appDbContext.SystemsInfos.Remove(system);
            _appDbContext.SaveChanges();
        }

        public async Task<SystemInfo> GetAsync(List<string> ids, int? limit)
        {
            SystemInfo system;
            if (limit != null)
            {
                system = await Task.FromResult(
                    _appDbContext.SystemsInfos
                    .Include(x => x.SystemReadings.Take(Convert.ToInt32(limit)))
                    .ThenInclude(x => x.Usage)
                    .Include(x => x.SystemReadings.Take(Convert.ToInt32(limit)))
                    .ThenInclude(x => x.SystemSpecs)
                    .FirstOrDefault(
                        x => x.SystemMacs.Split(";", StringSplitOptions.RemoveEmptyEntries).Intersect(ids).Count() > 0
                    )
                );
            }
            else
            {
                system = await Task.FromResult(
                    _appDbContext.SystemsInfos
                    .Include(x => x.SystemReadings)
                    .ThenInclude(x => x.Usage)
                    .Include(x => x.SystemReadings)
                    .ThenInclude(x => x.SystemSpecs)
                    .FirstOrDefault(
                        x => x.SystemMacs.Split(";", StringSplitOptions.RemoveEmptyEntries).Intersect(ids).Count() > 0
                    )
                );
            }
            if (system == null)
            {
                return null;
            }
            return system;
        }

        public async Task UpdateAsync(SystemInfo systemInfo, List<string> ids)
        {
            var system = await Task.FromResult(_appDbContext.SystemsInfos.FirstOrDefault(x => ids.Any(a => x.SystemMacs.Contains(a))));
            if (system == null)
            {
                throw new Exception("Unable to find systemInfo");
            }
            system.SystemName = systemInfo.SystemName;
            system.IsAuthorised = systemInfo.IsAuthorised;
            system.SystemMacs = systemInfo.SystemMacs;
            _appDbContext.SaveChanges();
            return;
        }
    }
}
