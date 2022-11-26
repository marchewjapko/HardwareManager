using HardwareMonitor.DataSource;
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
                var systemWindows = new SystemInfoWindows();
                switch (mode)
                {
                    case 0:
                        System.Timers.Timer timer = new();
                        timer.Elapsed += (sender, args) => OnTimer(systemWindows);
                        timer.Interval = 5000;
                        timer.Enabled = true;

                        Console.WriteLine("Press \'q\' to exit");
                        while (Console.Read() != 'q') ;
                        break;
                    case 1:
                        LoopOnKeyPress(systemWindows);
                        break;
                    default:
                        Console.WriteLine("Invalid mode");
                        break;
                }
            }
            else
            {
                var systemLinux = new SystemInfoLinux();
                switch (mode)
                {
                    case 0:
                        System.Timers.Timer timer = new();
                        timer.Elapsed += (sender, args) => OnTimer(systemLinux);
                        timer.Interval = 5000;
                        timer.Enabled = true;

                        Console.WriteLine("Press \'q\' to exit");
                        while (Console.Read() != 'q') ;
                        break;
                    case 1:
                        LoopOnKeyPress(systemLinux);
                        break;
                    default:
                        Console.WriteLine("Invalid mode");
                        break;
                }
            }
        }

        private static void OnTimer(SystemInfoWindows systemWindows)
        {
            var time = DateTime.Now;
            var str = systemWindows.GetSystemInfo().ToString();
            Console.Clear();
            Console.WriteLine(str);
            Console.WriteLine("Elapsed time: " + (DateTime.Now - time));
        }

        private static void OnTimer(SystemInfoLinux systemLinux)
        {
            var time = DateTime.Now;
            var str = systemLinux.GetSystemInfo().ToString();
            Console.Clear();
            Console.WriteLine(str);
            Console.WriteLine("Elapsed time: " + (DateTime.Now - time));
        }

        private static void LoopOnKeyPress(SystemInfoWindows systemWindows)
        {
            while (true)
            {
                var time = DateTime.Now;
                var str = systemWindows.GetSystemInfo().ToString();
                Console.Clear();
                Console.WriteLine(str);
                Console.WriteLine("Elapsed time: " + (DateTime.Now - time).TotalSeconds);
                Console.ReadKey();
            }
        }

        private static void LoopOnKeyPress(SystemInfoLinux systemLinux)
        {
            while (true)
            {
                var time = DateTime.Now;
                var str = systemLinux.GetSystemInfo().ToString();
                Console.Clear();
                Console.WriteLine(str);
                Console.WriteLine("Elapsed time: " + (DateTime.Now - time).TotalSeconds);
                Console.ReadKey();
            }
        }
    }
}