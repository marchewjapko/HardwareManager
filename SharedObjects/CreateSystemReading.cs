using System.Text;

namespace SharedObjects
{
    public class CreateSystemReading
    {
        public CreateUsage CreateUsage { get; set; }
        public CreateSystemSpecs CreateSystemSpecs { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            StringBuilder result = new("------------------------------------------------\n");
            result.Append(CreateUsage.ToString() + "------------------------------------------------\n");
            result.Append(CreateSystemSpecs.ToString());
            result.Append("Timestamp: " + Timestamp + "\n");
            return result.ToString();
        }
    }
}
