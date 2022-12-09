using HardwareMonitor.Core.Domain;
using Microsoft.EntityFrameworkCore;
using SystemMonitor.Core.Domain;

namespace HardwareMonitor.RestAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<SystemInfo> SystemsInfos { get; set; }
        public DbSet<SystemUsage> Usages { get; set; }
        public DbSet<SystemSpecs> SystemSpecs { get; set; }
        public DbSet<SystemReading> SystemReadings { get; set; }

        public DbSet<CpuPerCoreUsage> CpuPerCoreUsages { get; set; }
        public DbSet<DiskSpecs> DisksSpecs { get; set; }
        public DbSet<DiskUsage> DiskUsages { get; set; }
        public DbSet<NetworkSpecs> NetworksSpecs { get; set; }
        public DbSet<NetworkUsage> NetworkUsages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemReading>()
                .HasOne<SystemInfo>(x => x.SystemInfo)
                .WithMany(x => x.SystemReadings)
                .HasForeignKey(x => x.SystemInfoId);
        }
    }
}
