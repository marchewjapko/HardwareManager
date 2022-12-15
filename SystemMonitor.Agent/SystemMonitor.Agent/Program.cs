using SystemMonitor.DataSource;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.InteropServices;

namespace SystemMonitor.Agent
{
    internal class Program
    {
        static readonly HttpClient client = new HttpClient();
        static HubConnection connection;

        static async Task Main()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();
            var connectionString = config["ConnectionString"];

            connection = new HubConnectionBuilder()
               .WithUrl(new Uri(connectionString + "/systemInfoHub"))
               .WithAutomaticReconnect()
               .Build();
            await connection.StartAsync();

            if (!(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux)))
            {
                throw new Exception("Invalid OS platfom, supported platfroms: Windows, Linux");
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var systemWindows = new SystemInfoWindows();
                System.Timers.Timer timer = new();
                timer.Elapsed += async (sender, args) => await OnTimer(systemWindows);
                timer.Interval = 5000;
                timer.Enabled = true;

                Console.WriteLine("Press \'q\' to exit");
                while (Console.Read() != 'q') ;
            }
            else
            {
                var systemLinux = new SystemInfoLinux();
                System.Timers.Timer timer = new();
                timer.Elapsed += async (sender, args) => await OnTimer(systemLinux);
                timer.Interval = 5000;
                timer.Enabled = true;

                Console.WriteLine("Press \'q\' to exit");
                while (Console.Read() != 'q') ;
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
                var response = await connection.InvokeAsync<string>("AddSystem", system);
                Console.WriteLine("--------------------------\nResponse: " + response + "\n");
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
                var response = await connection.InvokeAsync<string>("AddSystem", system);
                Console.WriteLine("--------------------------\nResponse: " + response + "\n");
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
    }
}