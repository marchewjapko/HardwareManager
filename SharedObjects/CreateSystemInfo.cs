using System.Text;

namespace SharedObjects
{
    public class CreateSystemInfo
    {
        public List<string> SystemMacs { get; set; }
        public string SystemName { get; set; }
        public List<CreateSystemReading> CreateSystemReadings { get; set; }

        public override string ToString()
        {
            StringBuilder result = new();
            result.Append("System MACs: \n");
            foreach(var mac in SystemMacs)
            {
                result.Append("\t" + mac + "\n");
            }
            result.Append("System name: " + SystemName + "\n");
            result.Append(CreateSystemReadings[0].ToString());
            return result.ToString();
        }
    }
}
