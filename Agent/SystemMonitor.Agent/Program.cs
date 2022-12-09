using HardwareMonitor.DataSource;
using System;
using System.Net.Http.Json;
using System.Runtime.InteropServices;

namespace SystemMonitor.Agent
{
    internal class Program
    {
        static int mode = 1;
        static HttpClient client = new HttpClient();

        static async Task Main()
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
                        timer.Elapsed += async (sender, args) => await OnTimer(systemWindows);
                        timer.Interval = 5000;
                        timer.Enabled = true;

                        Console.WriteLine("Press \'q\' to exit");
                        while (Console.Read() != 'q') ;
                        break;
                    case 1:
                        await LoopOnKeyPress(systemWindows);
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
                        timer.Elapsed += async (sender, args) => await OnTimer(systemLinux);
                        timer.Interval = 5000;
                        timer.Enabled = true;

                        Console.WriteLine("Press \'q\' to exit");
                        while (Console.Read() != 'q') ;
                        break;
                    case 1:
                        await LoopOnKeyPress(systemLinux);
                        break;
                    default:
                        Console.WriteLine("Invalid mode");
                        break;
                }
            }
        }

        private async static Task OnTimer(SystemInfoWindows systemWindows)
        {
            var time = DateTime.Now;
            var system = systemWindows.GetSystemInfo();
            Console.Clear();
            Console.WriteLine(system.ToString());
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("http://192.168.1.2:8080/AddSystem", system);
                Console.WriteLine("--------------------------\nResponse: " + response.StatusCode + "\n");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Error:\n" + ex.InnerException.Message);
                }
                else
                {
                    Console.WriteLine("Something terrible happened:\n" + ex.ToString());
                }
            }
            Console.WriteLine("Elapsed time: " + (DateTime.Now - time));
        }

        private async static Task OnTimer(SystemInfoLinux systemLinux)
        {
            var time = DateTime.Now;
            var system = systemLinux.GetSystemInfo();
            Console.Clear();
            Console.WriteLine(system.ToString());
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("http://192.168.1.2:8080/AddSystem", system);
                Console.WriteLine("--------------------------\nResponse: " + response.StatusCode + "\n");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Error:\n" + ex.InnerException.Message);
                }
                else
                {
                    Console.WriteLine("Something terrible happened:\n" + ex.ToString());
                }
            }
            Console.WriteLine("Elapsed time: " + (DateTime.Now - time));
        }

        private async static Task LoopOnKeyPress(SystemInfoWindows systemWindows)
        {
            while (true)
            {
                var time = DateTime.Now;
                var system = systemWindows.GetSystemInfo();
                Console.Clear();
                Console.WriteLine(system.ToString());
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync("http://192.168.1.2:8080/AddSystem", system);
                    Console.WriteLine("--------------------------\nResponse: " + response.StatusCode + "\n");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Error:\n" + ex.InnerException.Message);
                    }
                    else
                    {
                        Console.WriteLine("Something terrible happened:\n" + ex.ToString());
                    }
                }
                Console.WriteLine("Elapsed time: " + (DateTime.Now - time).TotalSeconds);
                Console.ReadKey();
            }
        }

        private async static Task LoopOnKeyPress(SystemInfoLinux systemLinux)
        {
            while (true)
            {
                var time = DateTime.Now;
                var system = systemLinux.GetSystemInfo();
                Console.Clear();
                Console.WriteLine(system.ToString());
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync("http://192.168.1.2:8080/AddSystem", system);
                    Console.WriteLine("--------------------------\nResponse: " + response.StatusCode + "\n");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Error:\n" + ex.InnerException.Message);
                    }
                    else
                    {
                        Console.WriteLine("Something terrible happened:\n" + ex.ToString());
                    }
                }
                Console.WriteLine("Elapsed time: " + (DateTime.Now - time).TotalSeconds);
                Console.ReadKey();
            }
        }
    }
}