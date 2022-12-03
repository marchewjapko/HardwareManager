using System.Collections.Generic;

namespace HardwareMonitor.Core.Domain
{
    public class SystemInfo
    {
        public int Id { get; set; }
        public bool IsAuthorised { get; set; }
        public string SystemMacs { get; set; }
        public string SystemName { get; set; }

        public ICollection<SystemReading> SystemReadings{ get; set; }
    }
}
