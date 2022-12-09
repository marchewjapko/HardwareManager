using System.Text;

namespace SystemMonitor.SharedObjects
{
    public class CreateCpuPerCoreUsage
    {
        public string Instance { get; set; }
        public double Usage { get; set; }

        public override string ToString()
        {
            StringBuilder result = new();
            result.Append("\t" + Instance + " - " + Usage + "%\n");
            return result.ToString();
        }
    }
}
