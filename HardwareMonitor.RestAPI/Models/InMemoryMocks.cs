using HardwareMonitor.Core.Domain;

namespace HardwareMonitor.RestAPI.Models
{
    public static class InMemoryMocks
    {
        public static void AddMachineData(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetService<AppDbContext>();

            var system1 = new SystemInfo
            {
                Id = 0,
                IsAuthorised = true,
                SystemMacs = "00-D8-61-70-09-E3;00-15-5D-57-BA-D5",
                SystemName = "System Name#1"
            };

            db.SystemsInfos.Add(system1);

            db.SaveChanges();
        }
    }
}
