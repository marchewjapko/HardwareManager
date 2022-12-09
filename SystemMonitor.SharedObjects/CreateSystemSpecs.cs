using System.Text;
using SystemMonitor.SharedObjects;

namespace SharedObjects
{
    public class CreateSystemSpecs
    {
        public string OsNameVersion { get; set; }
        public string CpuInfo { get; set; }
        public int CpuCores { get; set; }
        public double TotalMemory { get; set; }
        public List<CreateNetworkSpecs> CreateNetworkSpecs { get; set; }
        public List<CreateDiskSpecs> CreateDiskSpecs { get; set; }

        public override string ToString()
        {
            StringBuilder result = new();
            result.Append("Operating system: " + OsNameVersion + "\n");
            result.Append("CPU: " + CpuInfo + "\n");
            result.Append("CPU cores: " + CpuCores + "\n");
            result.Append("Total RAM: " + Math.Round(TotalMemory / 1048576, 1) + " GB\n");
            result.Append("Network adapters: " + "\n");
            foreach (var specs in CreateNetworkSpecs)
            {
                result.Append(specs.ToString());
            }
            result.Append("Physical drives: " + "\n");
            foreach (var specs in CreateDiskSpecs)
            {
                result.Append(specs.ToString());
            }
            return result.ToString();
        }
    }
}
