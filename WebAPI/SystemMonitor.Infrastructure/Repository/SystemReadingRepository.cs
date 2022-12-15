using SystemMonitor.Core.Domain;
using SystemMonitor.Core.Repositories;
using SystemMonitor.WebAPI;

namespace SystemMonitor.Infrastructure.Repository
{
    public class SystemReadingRepository : ISystemReadingRepository
    {
        private readonly AppDbContext _appDbContext;
        public SystemReadingRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddAsync(List<SystemReading> systemReadings, int systemId)
        {
            var systemInfo = _appDbContext.SystemsInfos.FirstOrDefault(x => x.Id == systemId);
            foreach (var reading in systemReadings)
            {
                reading.SystemInfo = systemInfo;
            }
            _appDbContext.SystemReadings.AddRange(systemReadings);
            await _appDbContext.SaveChangesAsync();
            return;
        }
        public async Task<Task> DeleteAsync(DateTime? from, DateTime? to, int systemId)
        {
            var system = await Task.FromResult(_appDbContext.SystemsInfos.FirstOrDefault(x => x.Id == systemId));
            if (system == null)
            {
                return Task.FromException(new Exception("not-found"));
            }
            var readings = await Task.FromResult(_appDbContext.SystemReadings.Where(x => x.SystemInfoId == systemId));
            if (from != null)
            {
                readings = readings.Where(x => x.Timestamp >= from);
            }
            if(to != null)
            {
                readings = readings.Where(x => x.Timestamp <= to);
            }
            _appDbContext.SystemReadings.RemoveRange(readings);
            return Task.FromResult(_appDbContext.SaveChanges());
        }
    }
}
