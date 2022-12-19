using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemMonitor.Core.Domain;

namespace SystemMonitor.Core.Repositories
{
    public interface ISystemReadingRepository
    {
        Task AddAsync(List<SystemReading> systemReadings, int id);
        Task<IEnumerable<SystemReading>> GetReadings(DateTime? from, DateTime? to, int systemId);
        Task<Task> DeleteAsync(DateTime? from, DateTime? to, int id);
    }
}
