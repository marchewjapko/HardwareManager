using SystemMonitor.Infrastructure.DTO;
using SharedObjects;
using SystemMonitor.Infrastructure.DTO;

namespace SystemMonitor.Infrastructure.DTO
{
    public class SystemSpecsDTO
    {
        public string OsNameVersion { get; set; }
        public string CpuInfo { get; set; }
        public int CpuCores { get; set; }
        public double TotalMemory { get; set; }
        public List<NetworkSpecsDTO> NetworkSpecs { get; set; }
        public List<DiskSpecsDTO> DiskSpecs { get; set; }
    }
}
