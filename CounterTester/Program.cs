using DataSource;
using DataSource.Usage;
using System.Runtime.InteropServices;
using System.Timers;

namespace CounterTester
{
    internal class Program
    {
        static readonly HardwareMonitor hardwareMonitor = new();

        static void Main()
        {
            if (!(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux)))
            {
                throw new Exception("Invalid OS platfom, supported platfroms: Windows, Linux");
            }
            //System.Timers.Timer timer = new();
            //timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //timer.Interval = 5000;
            //timer.Enabled = true;

            //Console.WriteLine("Press \'q\' to exit");
            //while (Console.Read() != 'q');
            var machineSpecs = new MachineSpecs();
            Console.WriteLine(machineSpecs.ToString());
            Console.ReadKey();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.Clear();
            Console.WriteLine(hardwareMonitor.GetSystemUsage().ToString());
        }
    }
}