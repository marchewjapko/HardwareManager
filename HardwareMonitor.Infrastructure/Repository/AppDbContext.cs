using HardwareMonitor.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace HardwareMonitor.RestAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Machine> Machines { get; set; }
    }
}
