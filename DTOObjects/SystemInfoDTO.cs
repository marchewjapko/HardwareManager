using System.Text;

namespace SharedObjects
{
    public class SystemInfoDTO
    {
        public int Id { get; set; }
        public List<string> SystemMacs { get; set; }
        public string SystemName { get; set; }
        public bool? IsAuthorised { get; set; }

        public List<SystemSpecsDTO> SystemSpecsDTO { get; set; }
        public List<UsageDTO> UsageDTO { get; set; }

        public override string ToString()
        {
            StringBuilder result = new();
            result.Append("Machine ID: " + Id + "\n");
            result.Append("Machine MACs:\n");
            foreach(var mac in SystemMacs)
            {
                result.Append("\t" + mac.ToString() + "\n");
            }
            result.Append("Machine name: " + SystemName + "\n");
            result.Append("Is authorised: " + IsAuthorised);
            result.Append("\n----------------------------------------------------\n");
            result.Append(SystemSpecsDTO[0].ToString());
            result.Append("\n----------------------------------------------------\n");
            result.Append(UsageDTO[0].ToString());
            return result.ToString();
        }
    }
}