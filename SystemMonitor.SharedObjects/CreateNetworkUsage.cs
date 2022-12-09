using SharedObjects;
using System.Text;

namespace SystemMonitor.SharedObjects
{
    public class CreateNetworkUsage
    {
        public string AdapterName { get; set; }
        public double BytesSent { get; set; }
        public double BytesReceived { get; set; }

        public override string ToString()
        {
            StringBuilder result = new();
            result.Append("\tAdapter: " + AdapterName + "\n");
            result.Append("\tBytes sent: " + BytesSent + "\n");
            result.Append("\tBytes received: " + BytesReceived + "\n\n");
            return result.ToString();
        }
    }
}
