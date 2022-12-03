using HardwareMonitor.Core.Repositories;
using HardwareMonitor.Infrastructure.Repository;
using HardwareMonitor.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace HardwareMonitor.RestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ISystemInfoRepository, SystemInfoRepository>();
            builder.Services.AddScoped<ISystemInfoService, SystemInfoService>();

            builder.Services.AddScoped<ISystemReadingRepository, SystemReadingRepository>();
            builder.Services.AddScoped<ISystemReadingService, SystemReadingService>();

            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer("Server=system-monitor-db;Initial Catalog=systemMonitor;User=sa;Password=2620dvxje!ABC;TrustServerCertificate=True"));
            //builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer("Data Source=localhost;Initial Catalog=SystemMonitor;Integrated Security=True;TrustServerCertificate=True"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<AppDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }

            app.Run();
        }
    }
}