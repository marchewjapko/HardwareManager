using DataSource.Specs;
using DataSource.Usage.Linux;
using SharedObjects;

namespace SystemMonitor.DataSource.Reading
{
    public class SystemReadingLinux
    {
        UsageMonitorLinux usageMonitorLinux { get; set; }
        SystemSpecsLinux systemSpecsLinux { get; set; }
        public SystemReadingLinux()
        {
            usageMonitorLinux = new UsageMonitorLinux();
            systemSpecsLinux = new SystemSpecsLinux();
        }

        public CreateSystemReading GetSystemReading()
        {
            return new CreateSystemReading()
            {
                CreateSystemSpecs = systemSpecsLinux.GetMachineSpecs(),
                CreateUsage = usageMonitorLinux.GetSystemUsage(),
                Timestamp = DateTime.Now
            };
        }
    }
}
