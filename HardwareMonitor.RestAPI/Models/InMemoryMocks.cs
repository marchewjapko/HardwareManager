namespace HardwareMonitor.RestAPI.Models
{
    public static class InMemoryMocks
    {
        public static void AddMachineData(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetService<AppDbContext>();

            var machine1 = new Machine
            {
                Id = "machine1",
                IsAuthorised = true
            };

            var machine2 = new Machine
            {
                Id = "machine2",
                IsAuthorised = true
            };

            var machine3 = new Machine
            {
                Id = "machine3",
                IsAuthorised = false
            };

            db.Machines.Add(machine1);
            db.Machines.Add(machine2);
            db.Machines.Add(machine3);

            db.SaveChanges();
        }
    }
}
