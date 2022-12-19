using SystemMonitor.Core.Repositories;
using SystemMonitor.Infrastructure.Repository;
using SystemMonitor.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SystemMonitor.WebAPI.Hubs;

namespace SystemMonitor.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            var policyName = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: policyName,
                                  builder =>
                                  {
                                      builder
                                        .WithOrigins("http://192.168.1.2:3000", "https://192.168.193.20:3000", "http://localhost:3000")
                                        .AllowAnyMethod()
                                        .AllowCredentials()
                                        .AllowAnyHeader();
                                  });
            });

            builder.Services.AddSignalR(a =>
            {
                a.EnableDetailedErrors = true;
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "System monitor WebAPI", Version = "v1" });
                // here some other configurations maybe...
                options.AddSignalRSwaggerGen();
            });

            builder.Services.AddScoped<ISystemInfoRepository, SystemInfoRepository>();
            builder.Services.AddScoped<ISystemInfoService, SystemInfoService>();

            builder.Services.AddScoped<ISystemReadingRepository, SystemReadingRepository>();
            builder.Services.AddScoped<ISystemReadingService, SystemReadingService>();

            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer("Server=system-monitor-db;Initial Catalog=systemMonitor;User=sa;Password=2620dvxje!ABC;TrustServerCertificate=True"));
            //builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer("Data Source=localhost;Initial Catalog=SystemMonitor;Integrated Security=True;TrustServerCertificate=True"));

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors(policyName);
            app.UseAuthorization();
            app.MapControllers();
            app.UseRouting();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<AppDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SystemInfoHub>("/systemInfoHub");
            });

            app.Run();
        }
    }
}