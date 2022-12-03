using HardwareMonitor.Infrastructure.DTO;
using SharedObjects;

namespace HardwareMonitor.Infrastructure.DTO
{
    public class SystemSpecsDTO
    {
        public string OsNameVersion { get; set; }
        public string CpuInfo { get; set; }
        public int CpuCores { get; set; }
        public double TotalMemory { get; set; }
        public List<StringDoublePair> NetworkAdapters { get; set; }
        public List<StringDoublePair> Disks { get; set; }
    }
}
