using DataSource.Specs;
using DataSource.Usage.Linux;
using DataSource.Usage.Windows;
using System.Runtime.InteropServices;

namespace CounterTester
{
    internal class Program
    {
        static int mode = 0;

        static void Main()
        {
            if (!(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux)))
            {
                throw new Exception("Invalid OS platfom, supported platfroms: Windows, Linux");
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                HardwareMonitorWindows hardwareMonitor = new();
                MachineSpecsWindows machineSpecsWindows = new();
                switch (mode)
                {
                    case 0:
                        System.Timers.Timer timer = new();
                        timer.Elapsed += (sender, args) => OnTimer(hardwareMonitor, machineSpecsWindows);
                        timer.Interval = 5000;
                        timer.Enabled = true;

                        Console.WriteLine("Press \'q\' to exit");
                        while (Console.Read() != 'q') ;
                        break;
                    case 1:
                        LoopOnKeyPress(hardwareMonitor, machineSpecsWindows);
                        break;
                    default:
                        Console.WriteLine("Invalid mode");
                        break;
                }
            }
            else
            {
                HardwareMonitorLinux hardwareMonitor = new();
                MachineSpecsLinux machineSpecsLinux = new();
                switch (mode)
                {
                    case 0:
                        System.Timers.Timer timer = new();
                        timer.Elapsed += (sender, args) => OnTimer(hardwareMonitor, machineSpecsLinux);
                        timer.Interval = 5000;
                        timer.Enabled = true;

                        Console.WriteLine("Press \'q\' to exit");
                        while (Console.Read() != 'q') ;
                        break;
                    case 1:
                        LoopOnKeyPress(hardwareMonitor, machineSpecsLinux);
                        break;
                    default:
                        Console.WriteLine("Invalid mode");
                        break;
                }
            }
        }

        private static void OnTimer(HardwareMonitorWindows hardwareMonitor, MachineSpecsWindows machineSpecsWindows)
        {
            var time = DateTime.Now;
            var str = hardwareMonitor.GetSystemUsage().ToString();
            var strSpecs = machineSpecsWindows.GetMachineSpecs();
            Console.Clear();
            Console.WriteLine(strSpecs);
            Console.WriteLine("-------------------------------------");
            Console.WriteLine(str);
            Console.WriteLine("Elapsed time: " + (DateTime.Now - time));
        }

        private static void OnTimer(HardwareMonitorLinux hardwareMonitor, MachineSpecsLinux machineSpecsLinux)
        {
            var time = DateTime.Now;
            var str = hardwareMonitor.GetSystemUsage().ToString();
            var strSpecs = machineSpecsLinux.GetMachineSpecs();
            Console.Clear();
            Console.WriteLine(strSpecs);
            Console.WriteLine("-------------------------------------");
            Console.WriteLine(str);
            Console.WriteLine("Elapsed time: " + (DateTime.Now - time));
        }

        private static void LoopOnKeyPress(HardwareMonitorWindows hardwareMonitor, MachineSpecsWindows machineSpecsWindows)
        {
            while (true)
            {
                var time = DateTime.Now;
                var strUsage = hardwareMonitor.GetSystemUsage().ToString();
                var strSpecs = machineSpecsWindows.GetMachineSpecs();
                Console.Clear();
                Console.WriteLine(strSpecs);
                Console.WriteLine("-------------------------------------");
                Console.WriteLine(strUsage);
                Console.WriteLine("Elapsed time: " + (DateTime.Now - time).TotalSeconds);
                Console.ReadKey();
            }
        }

        private static void LoopOnKeyPress(HardwareMonitorLinux hardwareMonitor, MachineSpecsLinux machineSpecsLinux)
        {
            while (true)
            {
                var time = DateTime.Now;
                var str = hardwareMonitor.GetSystemUsage().ToString();
                var strSpecs = machineSpecsLinux.GetMachineSpecs();
                Console.Clear();
                Console.WriteLine(strSpecs);
                Console.WriteLine("-------------------------------------");
                Console.WriteLine(str);
                Console.WriteLine("Elapsed time: " + (DateTime.Now - time).TotalSeconds);
                Console.ReadKey();
            }
        }
    }
}