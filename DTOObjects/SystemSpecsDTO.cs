using System.Text;

namespace SharedObjects
{
    public class SystemSpecsDTO
    {
        public string OsNameVersion { get; set; }
        public string CpuInfo { get; set; }
        public int CpuCores { get; set; }
        public double TotalMemory { get; set; }
        public List<StringDoublePair> NetworkAdapters { get; set; }
        public List<StringDoublePair> Disks { get; set; }
        public DateTime Timestamp { get; set; }


        public SystemSpecsDTO(
            string osNameVersion, 
            string cpuInfo, 
            int cpuCores, 
            double totalMemory,
            List<StringDoublePair> networkAdapters, 
            List<StringDoublePair> disks, 
            DateTime timestamp
        )
        {
            OsNameVersion = osNameVersion;
            CpuInfo = cpuInfo;
            CpuCores = cpuCores;
            TotalMemory = totalMemory;
            NetworkAdapters = networkAdapters;
            Disks = disks;
            Timestamp = timestamp;
        }

        public override string ToString()
        {
            StringBuilder result = new();
            result.Append("Operating system: " + OsNameVersion + "\n");
            result.Append("CPU: " + CpuInfo + "\n");
            result.Append("CPU cores: " + CpuCores + "\n");
            result.Append("Total RAM: " + Math.Round(TotalMemory / 1048576, 1) + " GB\n");
            result.Append("Network adapters: " + "\n");
            foreach (var pair in NetworkAdapters)
            {
                result.Append("\t Adapter: " + pair.Item1 + " - " + pair.Item2 + "b/sec \n");
            }
            result.Append("Physical drives: " + "\n");
            foreach (var pair in Disks)
            {
                result.Append("\t Drive: " + pair.Item1 + " - " + pair.Item2 + " GB\n");
            }
            result.Append("Timestamp: " + Timestamp);
            return result.ToString();
        }
    }
}
