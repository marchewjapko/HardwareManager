using SystemMonitor.DataSource;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using SharedObjects;
using SystemMonitor.SharedObjects;
using Microsoft.Extensions.Logging;

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
               .ConfigureLogging(x =>
               {
                   x.SetMinimumLevel(LogLevel.Trace);
               })
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
            Random rnd = new Random();

            var test = new CreateSystemInfo();
            test.SystemMacs = new List<string>() { "testMAC1" };
            test.SystemName = "TestSystem1";

            var timeStamp = DateTime.Now.AddDays(-1);

            for (int x = 0; x < 864; x++)
            {
                var readings = new List<CreateSystemReading>();
                for (int i = 0; i < 50; i++)
                {
                    var usage = new CreateSystemUsage();
                    usage.CpuTotalUsage = rnd.Next(0, 100);
                    usage.CreateCpuPerCoreUsage = new List<CreateCpuPerCoreUsage>
            {
                new CreateCpuPerCoreUsage
                {
                    Instance = "0",
                    Usage = 0
                }
            };
                    usage.CreateDiskUsage = new List<CreateDiskUsage>
            {
                new CreateDiskUsage
                {
                    DiskName = "0",
                    Usage = 0
                }
            };
                    usage.MemoryUsage = rnd.Next(8192, 16384);
                    usage.CreateNetworkUsage = new List<CreateNetworkUsage>
            {
                new CreateNetworkUsage
                {
                    AdapterName = "0",
                    BytesReceived = 0,
                    BytesSent = 0,
                }
            };
                    usage.SystemUptime = 0;

                    var specs = new CreateSystemSpecs();
                    specs.OsNameVersion = "TEST OS";
                    specs.CpuInfo = "VERY NICE CPU";
                    specs.CpuCores = 8;
                    specs.TotalMemory = 33554432;
                    specs.CreateNetworkSpecs = new List<CreateNetworkSpecs>
            {
                new CreateNetworkSpecs
                {
                    AdapterName = "0",
                    Bandwidth = 0,
                }
            };

                    specs.CreateDiskSpecs = new List<CreateDiskSpecs>
            {
                new CreateDiskSpecs
                {
                    DiskName = "0",
                    DiskSize = 0,
                }
            };

                    readings.Add(new CreateSystemReading()
                    {
                        CreateUsage = usage,
                        CreateSystemSpecs = specs,
                        Timestamp = timeStamp
                    });

                    timeStamp = timeStamp.AddSeconds(2);
                }


                test.CreateSystemReadings = readings;

                var response = await connection.InvokeAsync<string>("AddSystem", test);
                Console.WriteLine("--------------------------\nResponse: " + response + "\n");
            }


            //if (!(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux)))
            //{
            //    throw new Exception("Invalid OS platfom, supported platfroms: Windows, Linux");
            //}
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //{
            //    var systemWindows = new SystemInfoWindows();
            //    await OnTimer(systemWindows);
            //    //System.Timers.Timer timer = new();
            //    //timer.Elapsed += async (sender, args) => await OnTimer(systemWindows);
            //    //timer.Interval = Convert.ToDouble(config["Interval"]);
            //    //timer.Enabled = true;

            //    Console.WriteLine("Press \'q\' to exit");
            //    while (Console.Read() != 'q') ;
            //}
            //else
            //{
            //    var systemLinux = new SystemInfoLinux();
            //    System.Timers.Timer timer = new();
            //    timer.Elapsed += async (sender, args) => await OnTimer(systemLinux);
            //    timer.Interval = Convert.ToDouble(config["Interval"]);
            //    timer.Enabled = true;

            //    Console.WriteLine("Press \'q\' to exit");
            //    while (Console.Read() != 'q') ;
            //}
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