using HardwareMonitor.Core.Domain;
using HardwareMonitor.Core.Repositories;
using HardwareMonitor.RestAPI;
using Microsoft.EntityFrameworkCore;

namespace HardwareMonitor.Infrastructure.Repository
{
    public class SystemReadingRepository : ISystemReadingRepository
    {
        private readonly AppDbContext _appDbContext;
        public SystemReadingRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddAsync(List<SystemReading> systemReadings, int id)
        {
            var systemInfo = _appDbContext.SystemsInfos.FirstOrDefault(x => x.Id == id);
            foreach (var reading in systemReadings)
            {
                reading.SystemInfo = systemInfo;
            }
            _appDbContext.SystemReadings.AddRange(systemReadings);
            await _appDbContext.SaveChangesAsync();
            return;
        }
        public async Task<Task> DeleteAsync(DateTime from, DateTime to, List<string> systemIds)
        {
            var system = await Task.FromResult(
                _appDbContext.SystemsInfos.ToList()
                    .FirstOrDefault(x => x.SystemMacs.Split(";", StringSplitOptions.RemoveEmptyEntries)
                        .Any(a => systemIds.Contains(a))
                    )
                );
            if (system == null)
            {
                return Task.FromException(new Exception("not-found"));
            }
            var readings = await Task.FromResult(
                _appDbContext.SystemsInfos
                    .Include(x => x.SystemReadings
                        .Where(a => a.SystemInfoId == system.Id && a.Timestamp >= from && a.Timestamp <= to)
                    ).FirstOrDefault(x => x.Id == system.Id).SystemReadings
                );
            _appDbContext.SystemReadings.RemoveRange(system.SystemReadings);
            return Task.FromResult(_appDbContext.SaveChanges());
        }
        public async Task<Task> DeleteAsync(int id)
        {
            var reading = await Task.FromResult(_appDbContext.SystemReadings.FirstOrDefault(x => x.Id == id));
            _appDbContext.SystemReadings.Remove(reading);
            return Task.FromResult(_appDbContext.SaveChanges());
        }
        public async Task<Task> DeleteAsync(List<string> systemIds)
        {
            var system = await Task.FromResult(
                _appDbContext.SystemsInfos.ToList()
                    .FirstOrDefault(x => x.SystemMacs.Split(";", StringSplitOptions.RemoveEmptyEntries)
                        .Any(a => systemIds.Contains(a))
                    )
                );
            if (system == null)
            {
                return Task.FromException(new Exception("not-found"));
            }
            var readings = await Task.FromResult(
                _appDbContext.SystemsInfos
                    .Include(x => x.SystemReadings
                        .Where(a => a.SystemInfoId == system.Id)
                    ).FirstOrDefault(x => x.Id == system.Id).SystemReadings
                );
            _appDbContext.SystemReadings.RemoveRange(readings);
            return Task.FromResult(_appDbContext.SaveChanges());
        }
    }
}
