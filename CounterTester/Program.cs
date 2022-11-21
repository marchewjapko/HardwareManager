using DataSource.Specs;
using DataSource.Usage.Linux;
using DataSource.Usage.Windows;
using HardwareMonitor.DataSource.Reading;
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
                var systemWindows = new SystemReadingWindows();
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
                var systemLinux = new SystemReadingLinux();
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

        private static void OnTimer(SystemReadingWindows systemWindows)
        {
            var time = DateTime.Now;
            var str = systemWindows.GetSystemReading().ToString();
            Console.Clear();
            Console.WriteLine(str);
            Console.WriteLine("Elapsed time: " + (DateTime.Now - time));
        }

        private static void OnTimer(SystemReadingLinux systemLinux)
        {
            var time = DateTime.Now;
            var str = systemLinux.GetSystemReading().ToString();
            Console.Clear();
            Console.WriteLine(str);
            Console.WriteLine("Elapsed time: " + (DateTime.Now - time));
        }

        private static void LoopOnKeyPress(SystemReadingWindows systemWindows)
        {
            while (true)
            {
                var time = DateTime.Now;
                var str = systemWindows.GetSystemReading().ToString();
                Console.Clear();
                Console.WriteLine(str);
                Console.WriteLine("Elapsed time: " + (DateTime.Now - time).TotalSeconds);
                Console.ReadKey();
            }
        }

        private static void LoopOnKeyPress(SystemReadingLinux systemLinux)
        {
            while (true)
            {
                var time = DateTime.Now;
                var str = systemLinux.GetSystemReading().ToString();
                Console.Clear();
                Console.WriteLine(str);
                Console.WriteLine("Elapsed time: " + (DateTime.Now - time).TotalSeconds);
                Console.ReadKey();
            }
        }
    }
}