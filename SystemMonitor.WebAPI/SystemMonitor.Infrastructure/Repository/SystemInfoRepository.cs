using Microsoft.EntityFrameworkCore;
using SystemMonitor.Core.Domain;
using SystemMonitor.Core.Repositories;
using SystemMonitor.WebAPI;

namespace SystemMonitor.Infrastructure.Repository
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
                return await Task.FromResult(_appDbContext.SystemsInfos
                    .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                        .ThenInclude(x => x.Usage)
                    .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                        .ThenInclude(x => x.Usage)
                            .ThenInclude(x => x.CpuPerCoreUsage)
                    .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                        .ThenInclude(x => x.Usage)
                            .ThenInclude(x => x.DiskUsage)
                    .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                        .ThenInclude(x => x.Usage)
                            .ThenInclude(x => x.NetworkUsage)
                    .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                        .ThenInclude(x => x.SystemSpecs)
                    .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                        .ThenInclude(x => x.SystemSpecs)
                            .ThenInclude(x => x.NetworkSpecs)
                    .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                        .ThenInclude(x => x.SystemSpecs)
                            .ThenInclude(x => x.DiskSpecs)
                );
            }
            return await Task.FromResult(_appDbContext.SystemsInfos
                .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                    .ThenInclude(x => x.Usage)
                .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                    .ThenInclude(x => x.Usage)
                        .ThenInclude(x => x.CpuPerCoreUsage)
                .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                    .ThenInclude(x => x.Usage)
                        .ThenInclude(x => x.DiskUsage)
                .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                    .ThenInclude(x => x.Usage)
                        .ThenInclude(x => x.NetworkUsage)
                .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                    .ThenInclude(x => x.SystemSpecs)
                .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                    .ThenInclude(x => x.SystemSpecs)
                        .ThenInclude(x => x.NetworkSpecs)
                .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                    .ThenInclude(x => x.SystemSpecs)
                        .ThenInclude(x => x.DiskSpecs)
            );
        }

        public async Task<Task> DeleteAsync(int id)
        {
            var system = await Task.FromResult(_appDbContext.SystemsInfos.FirstOrDefault(x => x.Id == id));
            if (system == null)
            {
                return Task.FromException(new Exception("not-found"));
            }
            await Task.FromResult(_appDbContext.SystemsInfos.Remove(system));
            return Task.FromResult(_appDbContext.SaveChanges());
        }

        public async Task<SystemInfo> GetAsync(List<string> macs, int? limit)
        {
            var system = await Task.FromResult(
                _appDbContext.SystemsInfos.ToList()
                    .FirstOrDefault(x => x.SystemMacs.Split(";", StringSplitOptions.RemoveEmptyEntries)
                        .Any(a => macs.Contains(a))
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
                            .ThenInclude(x => x.Usage)
                                .ThenInclude(x => x.CpuPerCoreUsage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                            .ThenInclude(x => x.Usage)
                                .ThenInclude(x => x.DiskUsage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                            .ThenInclude(x => x.Usage)
                                .ThenInclude(x => x.NetworkUsage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                            .ThenInclude(x => x.SystemSpecs)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                            .ThenInclude(x => x.SystemSpecs)
                                .ThenInclude(x => x.NetworkSpecs)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                            .ThenInclude(x => x.SystemSpecs)
                                .ThenInclude(x => x.DiskSpecs)
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
                            .ThenInclude(x => x.Usage)
                                .ThenInclude(x => x.CpuPerCoreUsage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                            .ThenInclude(x => x.Usage)
                                .ThenInclude(x => x.DiskUsage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                            .ThenInclude(x => x.Usage)
                                .ThenInclude(x => x.NetworkUsage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                            .ThenInclude(x => x.SystemSpecs)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                            .ThenInclude(x => x.SystemSpecs)
                                .ThenInclude(x => x.NetworkSpecs)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                            .ThenInclude(x => x.SystemSpecs)
                                .ThenInclude(x => x.DiskSpecs)
                        .FirstOrDefault(x => x.Id == system.Id)
                );
            }
        }

        public async Task<SystemInfo> GetAsync(int id, int? limit)
        {
            if (limit != null)
            {
                return await Task.FromResult(
                    _appDbContext.SystemsInfos
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                            .ThenInclude(x => x.Usage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                            .ThenInclude(x => x.Usage)
                                .ThenInclude(x => x.CpuPerCoreUsage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                            .ThenInclude(x => x.Usage)
                                .ThenInclude(x => x.DiskUsage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                            .ThenInclude(x => x.Usage)
                                .ThenInclude(x => x.NetworkUsage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                            .ThenInclude(x => x.SystemSpecs)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                            .ThenInclude(x => x.SystemSpecs)
                                .ThenInclude(x => x.NetworkSpecs)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp).Take(Convert.ToInt32(limit)))
                            .ThenInclude(x => x.SystemSpecs)
                                .ThenInclude(x => x.DiskSpecs)
                        .FirstOrDefault(x => x.Id == id)
                );
            }
            else
            {
                return await Task.FromResult(
                    _appDbContext.SystemsInfos
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                            .ThenInclude(x => x.Usage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                            .ThenInclude(x => x.Usage)
                                .ThenInclude(x => x.CpuPerCoreUsage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                            .ThenInclude(x => x.Usage)
                                .ThenInclude(x => x.DiskUsage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                            .ThenInclude(x => x.Usage)
                                .ThenInclude(x => x.NetworkUsage)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                            .ThenInclude(x => x.SystemSpecs)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                            .ThenInclude(x => x.SystemSpecs)
                                .ThenInclude(x => x.NetworkSpecs)
                        .Include(x => x.SystemReadings.OrderByDescending(x => x.Timestamp))
                            .ThenInclude(x => x.SystemSpecs)
                                .ThenInclude(x => x.DiskSpecs)
                        .FirstOrDefault(x => x.Id == id)
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
