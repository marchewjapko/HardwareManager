using DataSource;
using DataSource.Usage;
using System.Runtime.InteropServices;
using System.Timers;

namespace CounterTester
{
    internal class Program
    {
        static readonly HardwareMonitor hardwareMonitor = new();
        //static readonly MachineSpecs machineSpecs = new();


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

            LoopOnKeyPressUsage();
            //LoopOnKeyPressSpecs();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var time = DateTime.Now;
            var str = hardwareMonitor.GetSystemUsage().ToString();
            Console.Clear();
            Console.WriteLine(str);
            Console.WriteLine("Elapsed time: " + (DateTime.Now - time));
            Console.ReadKey();
        }

        private static void LoopOnKeyPressUsage()
        {
            while (true)
            {
                var time = DateTime.Now;
                var str = hardwareMonitor.GetSystemUsage().ToString();
                //Console.Clear();
                //Console.WriteLine(str);
                Console.WriteLine((DateTime.Now - time).TotalSeconds);
                Console.ReadKey();
            }
        }

        //private static void LoopOnKeyPressSpecs()
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