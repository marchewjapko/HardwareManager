using HardwareMonitor.Core.Repositories;
using HardwareMonitor.Infrastructure.Repository;
using HardwareMonitor.Infrastructure.Services;
using HardwareMonitor.RestAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

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

            builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("InMemoryDB"));

            builder.Services.AddScoped<ISystemInfoRepository, SystemInfoRepository>();
            builder.Services.AddScoped<ISystemInfoService, SystemInfoService>();

            builder.Services.AddScoped<IUsageRepository, UsageRepository>();
            builder.Services.AddScoped<IUsageService, UsageService>();

            builder.Services.AddScoped<ISystemSpecsRepository, SystemSpecsRepository>();
            //builder.Services.AddScoped<IMachineService, MachineService>();

            var app = builder.Build();

            InMemoryMocks.AddMachineData(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}