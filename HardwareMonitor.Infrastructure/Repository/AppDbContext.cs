using HardwareMonitor.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace HardwareMonitor.RestAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<SystemInfo> SystemsInfos { get; set; }
        public DbSet<Usage> Usages { get; set; }
        public DbSet<SystemSpecs> SystemSpecs { get; set; }
        public DbSet<SystemReading> SystemReadings { get; set; }
    }
}
