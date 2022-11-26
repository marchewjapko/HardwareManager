using HardwareMonitor.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardwareMonitor.Core.Repositories
{
    public interface ISystemReadingRepository
    {
        Task AddAsync(List<SystemReading> systemReadings, int id);
    }
}
