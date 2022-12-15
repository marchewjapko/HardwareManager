using DataSource.Specs;
using DataSource.Usage.Windows;
using SharedObjects;
using System.Management;

namespace SystemMonitor.DataSource.Reading
{
    public class SystemReadingWindows
    {
        UsageMonitorWindows usageMonitorWindows { get; set; }
        SystemSpecsWindows systemSpecsWindows { get; set; }
        public SystemReadingWindows()
        {
            usageMonitorWindows = new UsageMonitorWindows();
            systemSpecsWindows = new SystemSpecsWindows();
        }

        public CreateSystemReading GetSystemReading()
        {
            return new CreateSystemReading()
            {
                CreateSystemSpecs = systemSpecsWindows.GetMachineSpecs(),
                CreateUsage = usageMonitorWindows.GetSystemUsage(),
                Timestamp = DateTime.Now
            };
        }
    }
}
