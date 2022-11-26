using HardwareMonitor.Core.Domain;
using SharedObjects;

namespace HardwareMonitor.Infrastructure.DTO.Conversions
{
    public static class SystemSpecsConversions
    {
        public static SystemSpecsDTO ToDTO(this SystemSpecs systemSpecs)
        {
            return new SystemSpecsDTO()
            {
                OsNameVersion = systemSpecs.OsNameVersion,
                CpuInfo = systemSpecs.CpuInfo,
                CpuCores = systemSpecs.CpuCores,
                TotalMemory = systemSpecs.TotalMemory,
                NetworkAdapters = ParseStringDouble(systemSpecs.NetworkAdapters),
                Disks = ParseStringDouble(systemSpecs.Disks),
            };

        }
        public static SystemSpecs ToDomain(this CreateSystemSpecs createSystemSpecs)
        {
            return new SystemSpecs()
            {
                OsNameVersion = createSystemSpecs.OsNameVersion,
                CpuInfo = createSystemSpecs.CpuInfo,
                CpuCores = createSystemSpecs.CpuCores,
                TotalMemory = createSystemSpecs.TotalMemory,
                NetworkAdapters = EncodeTuple(createSystemSpecs.NetworkAdapters),
                Disks = EncodeTuple(createSystemSpecs.Disks)
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
