﻿using DataSource.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataSource
{
    public class MachineSpecs
    {
        private readonly string machineName;
        private readonly string osNameVersion;
        private readonly string cpuInfo;
        private readonly int cpuCores;
        private readonly double totalMemory;
        private readonly List<(string name, float bandwidth)> networkAdapters;
        private readonly List<(string name, int size)> physicalDisks;

        public MachineSpecs()
        {
            machineName = Environment.MachineName;
            osNameVersion = RuntimeInformation.OSDescription;
            cpuInfo = GetCpuInfo();
            cpuCores = Environment.ProcessorCount;
            totalMemory = GetTotalMemory();
            networkAdapters = GetNetworkAdapters();
            physicalDisks = GetPhysicalDisks();
        }

        public MachineSpecs GetMachineSpecs()
        {
            return this;
        }

        public override string ToString()
        {
            StringBuilder result = new("Machine name: " + machineName + "\n");
            result.Append("Operating system: " + osNameVersion + "\n");
            result.Append("CPU: " + cpuInfo + "\n");
            result.Append("CPU cores: " + cpuCores + "\n");
            result.Append("Total RAM: " + Math.Round(totalMemory / 1048576, 1) + " GB\n");
            result.Append("Network adapters: " + "\n");
            foreach (var (name, bandwidth) in networkAdapters)
            {
                result.Append("\t Adapter: " + name + " - " + bandwidth + "b/sec \n");
            }
            result.Append("Physical drives: " + "\n");
            foreach (var (name, size) in physicalDisks)
            {
                result.Append("\t Drive: " + name + " - " + size + " GB\n");
            }
            return result.ToString();
        }

        private static string GetCpuInfo()
        {
            string result = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ManagementObjectSearcher cpuObjectSearcher = new("root\\CIMV2", "SELECT * FROM Win32_Processor");
                result = cpuObjectSearcher.Get().OfType<ManagementObject>().FirstOrDefault()["Name"].ToString();
            }
            else
            {
                var command = new ProcessStartInfo("cat")
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"cat /proc/cpuinfo | grep 'model name' | uniq\"",
                    RedirectStandardOutput = true
                };
                var commandOutput = "";
                using (var process = Process.Start(command))
                {
                    if (process == null)
                    {
                        throw new Exception("Error when executing process: " + command.Arguments);
                    }
                    commandOutput = process.StandardOutput.ReadToEnd();
                }
                result = commandOutput[(commandOutput.IndexOf(':') + 1)..][1..^1];
            }
            return result;
        }

        private static double GetTotalMemory()
        {
            double result = 0;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ManagementObjectSearcher memoryObjectSearcher = new("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                result = Convert.ToDouble(memoryObjectSearcher.Get().OfType<ManagementObject>().FirstOrDefault()["TotalVisibleMemorySize"]);
            }
            else
            {
                var command = new ProcessStartInfo("cat")
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"cat /proc/meminfo | grep 'MemTotal'\"",
                    RedirectStandardOutput = true
                };
                var commandOutput = "";
                using (var process = Process.Start(command))
                {
                    if (process == null)
                    {
                        throw new Exception("Error when executing process: " + command.Arguments);
                    }
                    commandOutput = process.StandardOutput.ReadToEnd();
                }
                result = Convert.ToDouble(commandOutput.Split(" ", StringSplitOptions.RemoveEmptyEntries)[^2]);
            }
            return result;
        }

        private static List<(string name, float bandwidth)> GetNetworkAdapters()
        {
            var result = new List<(string, float)>();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var category = new PerformanceCounterCategory("Network Interface");
                string[] instances = category.GetInstanceNames();
                foreach (var instance in instances)
                {
                    var bandwidth = new PerformanceCounter("Network Interface", "Current Bandwidth", instance);
                    result.Add((instance, bandwidth.NextValue()));
                }
            }
            else
            {
                var adapters = LinuxNetworkHelpers.GetAllNetworkAdapters();
                foreach (var adapter in adapters)
                {
                    var usage = LinuxNetworkHelpers.GetEthernetBandwidth(adapter);
                    if (usage == "")
                    {
                        usage = LinuxNetworkHelpers.GetWirelessBandwidth(adapter);
                        usage = usage.Split("\n").First(x => x.Contains("Current")).Split(" ", StringSplitOptions.RemoveEmptyEntries).First(x => x.Contains("Rate"));
                        var value = (float)0;
                        if (usage.Contains(':'))
                        {
                            value = float.Parse(usage[(usage.IndexOf(':') + 1)..]) * 1048576;
                        }
                        else
                        {
                            value = float.Parse(usage[(usage.IndexOf('=') + 1)..]) * 1048576;
                        }
                        result.Add((adapter, value));
                    }
                    else
                    {
                        var value = float.Parse(Regex.Replace(usage.Split(" ")[^1], "[^0-9]", "")) * 1048576;
                        result.Add((adapter, value));
                    }
                }
            }
            return result;
        }
        private static List<(string name, int size)> GetPhysicalDisks()
        {
            var result = new List<(string name, int size)>();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ManagementObjectSearcher diskObjectSearcher = new("root\\CIMV2", "SELECT * FROM Win32_LogicalDisk");
                foreach (var disk in diskObjectSearcher.Get())
                {
                    result.Add((disk["Name"].ToString(), Convert.ToInt32(Convert.ToInt64(disk["Size"]) / 1073741824)));
                }
            }
            else
            {
                var command = new ProcessStartInfo("lsblk")
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"lsblk -dno NAME,SIZE\"",
                    RedirectStandardOutput = true
                };
                var commandOutput = "";
                using (var process = Process.Start(command))
                {
                    if (process == null)
                    {
                        throw new Exception("Error when executing process: " + command.Arguments);
                    }
                    commandOutput = process.StandardOutput.ReadToEnd();
                }
                foreach (var line in commandOutput.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                {
                    var size = float.Parse(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1][..^1].Replace(',', '.'));
                    result.Add((line.Split(" ")[0], Convert.ToInt32(size)));
                }
            }
            return result;
        }
    }
}
