using System.Text;

namespace SystemMonitor.SharedObjects
{
    public class CreateNetworkSpecs
    {
        public string AdapterName { get; set; }
        public double Bandwidth { get; set; }

        public override string ToString()
        {
            StringBuilder result = new();
            result.Append("\tAdapter: " + AdapterName + "\n");
            result.Append("\tBandwidth: " + Bandwidth + "\n\n");
            return result.ToString();
        }
    }
}
