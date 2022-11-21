using SharedObjects;

namespace HardwareMonitor.Infrastructure.Commands
{
    public class CreateSystemInfo
    {
        public List<string> SystemMacs { get; set; }
        public string SystemName { get; set; }

        public List<CreateUsage> CreateUsage { get; set; }
        public List<SystemSpecsDTO> SystemSpecs { get; set; }
    }
}
