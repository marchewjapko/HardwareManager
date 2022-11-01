using DataSource;
using DataSource.Usage.Windows;
using DataSource.Usage.Linux;

using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Timers;

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
                switch (mode)
                {
                    case 0:
                        System.Timers.Timer timer = new();
                        timer.Elapsed += (sender, args) => UsageTimer(sender, args, hardwareMonitor);
                        timer.Interval = 5000;
                        timer.Enabled = true;

                        Console.WriteLine("Press \'q\' to exit");
                        while (Console.Read() != 'q') ;
                        break;
                    case 1:
                        LoopOnKeyPressUsage(hardwareMonitor);
                        break;
                    default:
                        Console.WriteLine("Invalid mode");
                        break;
                }
            }
            else
            {
                HardwareMonitorLinux hardwareMonitor = new();
                switch (mode)
                {
                    case 0:
                        System.Timers.Timer timer = new();
                        timer.Elapsed += (sender, args) => UsageTimer(sender, args, hardwareMonitor);
                        timer.Interval = 5000;
                        timer.Enabled = true;

                        Console.WriteLine("Press \'q\' to exit");
                        while (Console.Read() != 'q') ;
                        break;
                    case 1:
                        LoopOnKeyPressUsage(hardwareMonitor);
                        break;
                    default:
                        Console.WriteLine("Invalid mode");
                        break;
                }
            }
        }

        private static void UsageTimer(object source, ElapsedEventArgs e, HardwareMonitorWindows hardwareMonitor)
        {
            var time = DateTime.Now;
            var str = hardwareMonitor.GetSystemUsage().ToString();
            Console.Clear();
            Console.WriteLine(str);
            Console.WriteLine("Elapsed time: " + (DateTime.Now - time));
            Console.ReadKey();
        }

        private static void UsageTimer(object source, ElapsedEventArgs e, HardwareMonitorLinux hardwareMonitor)
        {
            var time = DateTime.Now;
            var str = hardwareMonitor.GetSystemUsage().ToString();
            Console.Clear();
            Console.WriteLine(str);
            Console.WriteLine("Elapsed time: " + (DateTime.Now - time));
            Console.ReadKey();
        }

        private static void LoopOnKeyPressUsage(HardwareMonitorWindows hardwareMonitor)
        {
            while (true)
            {
                var time = DateTime.Now;
                var str = hardwareMonitor.GetSystemUsage().ToString();
                Console.Clear();
                Console.WriteLine(str);
                Console.WriteLine("Elapsed time: " + (DateTime.Now - time).TotalSeconds);
                Console.ReadKey();
            }
        }

        private static void LoopOnKeyPressUsage(HardwareMonitorLinux hardwareMonitor)
        {
            while (true)
            {
                var time = DateTime.Now;
                var str = hardwareMonitor.GetSystemUsage().ToString();
                Console.Clear();
                Console.WriteLine(str);
                Console.WriteLine("Elapsed time: " + (DateTime.Now - time).TotalSeconds);
                Console.ReadKey();
            }
        }

        //private static void LoopOnKeyPressSpecs(HardwareMonitorWindows hardwareMonitor)
        //{
        //    while (true)
        //    {
        //        Console.Clear();
        //        var time = DateTime.Now;
        //        Console.WriteLine(machineSpecs.ToString());
        //        Console.WriteLine("Elapsed time: " + (DateTime.Now - time).TotalSeconds);
        //        Console.ReadKey();
        //    }
        //}

        //private static void LoopOnKeyPressSpecs(HardwareMonitorLinux hardwareMonitor)
        //{
        //    while (true)
        //    {
        //        Console.Clear();
        //        var time = DateTime.Now;
        //        Console.WriteLine(machineSpecs.ToString());
        //        Console.WriteLine("Elapsed time: " + (DateTime.Now - time).TotalSeconds);
        //        Console.ReadKey();
        //    }
        //}
    }
}