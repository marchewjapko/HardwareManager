using Microsoft.EntityFrameworkCore;
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
            if (to != null)
            {
                readings = readings.Where(x => x.Timestamp <= to);
            }
            _appDbContext.SystemReadings.RemoveRange(readings);
            return Task.FromResult(_appDbContext.SaveChanges());
        }

        public async Task<IEnumerable<SystemReading>> GetReadings(DateTime? from, DateTime? to, int systemId)
        {
            var readings = await Task.FromResult(_appDbContext.SystemReadings
                .Include(x => x.Usage)
                    .ThenInclude(x => x.CpuPerCoreUsage)
                .Include(x => x.Usage)
                    .ThenInclude(x => x.DiskUsage)
                .Include(x => x.Usage)
                    .ThenInclude(x => x.NetworkUsage)
                .Include(x => x.SystemSpecs)
                    .ThenInclude(x => x.NetworkSpecs)
                .Include(x => x.SystemSpecs)
                    .ThenInclude(x => x.DiskSpecs)
                .AsNoTracking()
                .Where(x => x.SystemInfoId == systemId));

            if (from != null)
            {
                readings = readings.Where(x => DateTime.Compare(x.Timestamp, (DateTime)from) > 0);
            }
            if (to != null)
            {
                readings = readings.Where(x => DateTime.Compare(x.Timestamp, (DateTime)from) < 0);
            }
            int readingsCount = readings.Count();

            if (readingsCount < 720)
            {
                return await Task.FromResult(readings.OrderBy(x => x.Timestamp));
            }

            int index = 0;
            int readingsInGroup;
            if (readingsCount < 1440)
            {
                readingsInGroup = (int)Math.Floor(readingsCount / 360.0);
            }
            else
            {
                readingsInGroup = (int)Math.Floor(readingsCount / 720.0);
            }
            var groupedReadigs = readings.OrderBy(x => x.Timestamp).AsEnumerable().GroupBy(x => index++ / readingsInGroup);
            return await Task.FromResult(groupedReadigs.SelectMany(x => x.Take(1)));
        }
    }
}
