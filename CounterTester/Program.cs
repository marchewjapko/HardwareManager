using DataSource;
using System.Timers;

namespace CounterTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var systemInfo = new SystemInfo();
                Console.WriteLine(systemInfo.ToString());
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}