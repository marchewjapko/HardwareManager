using HardwareMonitor.Core.Domain;
using HardwareMonitor.Core.Repositories;
using HardwareMonitor.RestAPI;

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
    }
}
