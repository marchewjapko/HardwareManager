namespace SystemMonitor.Infrastructure.DTO
{
    public class SystemInfoDTO
    {
        public int Id { get; set; }
        public bool? IsAuthorised { get; set; }
        public List<string> SystemMacs { get; set; }
        public string SystemName { get; set; }

        public List<SystemReadingDTO> SystemReadingDTOs { get; set; }
    }
}