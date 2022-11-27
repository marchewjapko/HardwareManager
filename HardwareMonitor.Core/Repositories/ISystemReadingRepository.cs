using HardwareMonitor.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardwareMonitor.Core.Repositories
{
    public interface ISystemReadingRepository
    {
        Task AddAsync(List<SystemReading> systemReadings, int id);
        Task<Task> DeleteAsync(DateTime from, DateTime to, List<string> systemIds);
        Task<Task> DeleteAsync(int id);
        Task<Task> DeleteAsync(List<string> systemIds);
    }
}
