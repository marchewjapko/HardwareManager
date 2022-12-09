using System.Text;

namespace SystemMonitor.SharedObjects
{
    public class CreateDiskUsage
    {
        public string DiskName { get; set; }
        public double Usage { get; set; }

        public override string ToString()
        {
            StringBuilder result = new();
            result.Append("\tDisk: " + DiskName + "\n");
            result.Append("\tUsage: " + Usage + "\n\n");
            return result.ToString();
        }
    }
}
