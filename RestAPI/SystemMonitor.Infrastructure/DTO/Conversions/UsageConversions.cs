using HardwareMonitor.Core.Domain;
using SharedObjects;

namespace HardwareMonitor.Infrastructure.DTO.Conversions
{
    public static class UsageConversions
    {
        public static UsageDTO ToDTO(this Usage usage)
        {
            return new UsageDTO()
            {
                CpuTotalUsage = usage.CpuTotalUsage,
                CpuPerCoreUsage = ParseStringDouble(usage.CpuPerCoreUsage),
                DiskUsage = ParseStringDouble(usage.DiskUsage),
                MemoryUsage = usage.MemoryUsage,
                BytesReceived = ParseStringDouble(usage.BytesReceived),
                BytesSent = ParseStringDouble(usage.BytesSent),
                SystemUptime = usage.SystemUptime,
            };
        }
        public static Usage ToDomain(this UsageDTO usageDTO)
        {
            return new Usage()
            {
                CpuTotalUsage = usageDTO.CpuTotalUsage,
                CpuPerCoreUsage = String.Join(';', usageDTO.CpuPerCoreUsage),
                DiskUsage = String.Join(';', usageDTO.DiskUsage),
                MemoryUsage = usageDTO.MemoryUsage,
                BytesReceived = String.Join(';', usageDTO.BytesReceived),
                BytesSent = String.Join(';', usageDTO.BytesSent),
                SystemUptime = usageDTO.SystemUptime
            };
        }
        public static Usage ToDomain(this CreateUsage createUsage)
        {
            return new Usage()
            {
                CpuTotalUsage = createUsage.CpuTotalUsage,
                CpuPerCoreUsage = EncodeTuple(createUsage.CpuPerCoreUsage),
                DiskUsage = EncodeTuple(createUsage.DiskUsage),
                MemoryUsage = createUsage.MemoryUsage,
                BytesReceived = EncodeTuple(createUsage.BytesReceived),
                BytesSent = EncodeTuple(createUsage.BytesSent),
                SystemUptime = createUsage.SystemUptime,
            };
        }
        private static List<StringDoublePair> ParseStringDouble(string tuple)
        {
            var result = new List<StringDoublePair>();
            var splitTuple = tuple.Split(";");
            for (int i = 0; i < splitTuple.Length - 1; i += 2)
            {
                result.Add(new StringDoublePair()
                {
                    Item1 = splitTuple[i],
                    Item2 = Convert.ToDouble(splitTuple[i + 1])
                });
            }
            return result;
        }
        private static string EncodeTuple(List<StringDoublePair> doubleTouple)
        {
            var result = "";
            foreach (var pair in doubleTouple)
            {
                result += pair.Item1 + ";" + pair.Item2 + ";";
            }
            return result;
        }
    }
}
