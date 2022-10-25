using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class SystemInfo
    {
        public double TotalCpuUsage;
        public List<(string name, double usage)> CoreUsage = new();
        public List<(string name, double usage)> DiskUsage = new();
        public double MemoryUsage;
        public List<(string name, double bandwidth)> Bandwidth = new();
        public List<(string name, double bytes)> BytesSent = new();
        public List<(string name, double bytes)> BytesReceived = new();
        public double SystemUptime;
        public double SystemCalls;

        [SupportedOSPlatform("windows")]
        public SystemInfo()
        {
            TotalCpuUsage = CPUInfo.GetCpuTotalUsage();
            CoreUsage = CPUInfo.GetCpuPerCoreUsage();
            DiskUsage = DiskInfo.GetDiskUsage();
            MemoryUsage = MemoryInfo.GetRemainingMemory();
            Bandwidth = NetworkInfo.GetBandwidth();
            BytesSent = NetworkInfo.GetBytesSent();
            BytesReceived = NetworkInfo.GetBytesReceived();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder("Total CPU usage: " + this.TotalCpuUsage + "\n");
            result.Append("Per core usage:\n");
            foreach(var core in CoreUsage)
            {
                result.Append("\t" + core.name + " - " + core.usage + "\n");
            }
            result.Append("Disk usage:\n");
            foreach(var disk in DiskUsage)
            {
                result.Append("\tDisk: " + disk.name + " - " + disk.usage + "\n");
            }
            result.Append("Avaible memory: " + MemoryUsage + "\n");
            result.Append("Bandwidths:\n");
            foreach(var bandwidth in Bandwidth)
            {
                result.Append("\tAdapter: " + bandwidth.name + " - " + bandwidth.bandwidth + "\n");
            }
            result.Append("Bytes sent:\n");
            foreach (var byteSent in BytesSent)
            {
                result.Append("\tAdapter: " + byteSent.name + " - " + byteSent.bytes + "\n");
            }
            result.Append("Bytes received:\n");
            foreach (var byteSent in BytesReceived)
            {
                result.Append("\tAdapter: " + byteSent.name + " - " + byteSent.bytes + "\n");
            }
            result.Append("System uptime: " + MemoryUsage + "\n");
            result.Append("System calls: " + SystemCalls);
            return result.ToString();
        }
    }
}
