namespace SystemMonitor.Infrastructure.Commands
{
    public class UpdateSystemInfo
    {
        public bool IsAuthorised { get; set; }
        public List<string> SystemMacs { get; set; }
        public string SystemName { get; set; }
    }
}
