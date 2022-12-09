using System.Text;

namespace SystemMonitor.SharedObjects
{
    public class CreateDiskSpecs
    {
        public string DiskName { get; set; }
        public double DiskSize { get; set; }

        public override string ToString()
        {
            StringBuilder result = new();
            result.Append("\tDisk: " + DiskName + "\n");
            result.Append("\tSize: " + DiskSize + "\n\n");
            return result.ToString();
        }
    }
}
