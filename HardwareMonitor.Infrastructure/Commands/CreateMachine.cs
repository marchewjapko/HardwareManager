namespace HardwareMonitor.Infrastructure.Commands
{
    public class CreateMachine
    {
        public string MachineId { get; set; }
        public string MachineName { get; set; }
        public bool IsAuthorised { get; set; }
    }
}
