using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;
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
                .Where(x => x.SystemInfoId == systemId));

            if (from != null)
            {
                readings = readings.Where(x => DateTime.Compare(x.Timestamp, (DateTime)from) > 0);
            }
            if (to != null)
            {
                readings = readings.Where(x => DateTime.Compare(x.Timestamp, (DateTime)from) < 0);
            }

            if (readings.Count() > 40000)
            {
                var result = readings.ToList()
                    .GroupBy(x => Round(x.Timestamp, TimeSpan.FromMinutes(1)))
                    .SelectMany(a => a.Where(b => b.Usage.CpuTotalUsage == a.Max(c => c.Usage.CpuTotalUsage)));
                return await Task.FromResult(result.OrderBy(x => x.Timestamp));
            }

            return await Task.FromResult(readings.OrderBy(x => x.Timestamp));
        }

        private DateTime Round(DateTime date, TimeSpan interval)
        {
            return new DateTime(
                (long)Math.Floor(date.Ticks / (double)interval.Ticks) * interval.Ticks
            );
        }
    }
}
