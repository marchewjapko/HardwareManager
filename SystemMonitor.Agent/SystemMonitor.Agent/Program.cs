using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using SystemMonitor.DataSource;

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
            try
            {
                await connection.StartAsync();
            }
            catch
            {
                Console.WriteLine("Unable to connect to server, check connection (" + connectionString + ")");
                Console.WriteLine("Press any key to exit...");
                Console.Read();
                return;
            }

            if (!(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux)))
            {
                throw new Exception("Invalid OS platfom, supported platfroms: Windows, Linux");
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var systemWindows = new SystemInfoWindows();
                await OnTimer(systemWindows);
                System.Timers.Timer timer = new();
                timer.Elapsed += async (sender, args) => await OnTimer(systemWindows);
                timer.Interval = Convert.ToDouble(config["Interval"]);
                timer.Enabled = true;

                Console.WriteLine("Press \'q\' to exit");
                while (Console.Read() != 'q');
            }
            else
            {
                var systemLinux = new SystemInfoLinux();
                System.Timers.Timer timer = new();
                timer.Elapsed += async (sender, args) => await OnTimer(systemLinux);
                timer.Interval = Convert.ToDouble(config["Interval"]);
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