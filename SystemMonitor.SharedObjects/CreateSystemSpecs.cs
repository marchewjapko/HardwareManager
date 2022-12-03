using System.Text;

namespace SharedObjects
{
    public class CreateSystemSpecs
    {
        public string OsNameVersion { get; set; }
        public string CpuInfo { get; set; }
        public int CpuCores { get; set; }
        public double TotalMemory { get; set; }
        public List<StringDoublePair> NetworkAdapters { get; set; }
        public List<StringDoublePair> Disks { get; set; }

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
            return result.ToString();
        }
    }
}
