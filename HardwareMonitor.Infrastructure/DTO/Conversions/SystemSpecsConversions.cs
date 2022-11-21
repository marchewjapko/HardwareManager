using HardwareMonitor.Core.Domain;
using HardwareMonitor.Infrastructure.Commands;
using SharedObjects;

namespace HardwareMonitor.Infrastructure.DTO.Conversions
{
    public static class SystemSpecsConversions
    {
        public static SystemSpecsDTO ToDTO(this SystemSpecs systemSpecs)
        {
            return new SystemSpecsDTO(
                systemSpecs.OsNameVersion,
                systemSpecs.CpuInfo,
                systemSpecs.CpuCores,
                systemSpecs.TotalMemory,
                ParseStringDouble(systemSpecs.NetworkAdapters),
                ParseStringDouble(systemSpecs.Disks),
                systemSpecs.Timestamp

            );
        }
        public static SystemSpecs ToDomain(this SystemSpecsDTO systemSpecsDTO)
        {
            return new SystemSpecs()
            {
                OsNameVersion = systemSpecsDTO.OsNameVersion,
                CpuInfo = systemSpecsDTO.CpuInfo,
                CpuCores = systemSpecsDTO.CpuCores,
                TotalMemory = systemSpecsDTO.TotalMemory,
                NetworkAdapters = EncodeTuple(systemSpecsDTO.NetworkAdapters),
                Disks = EncodeTuple(systemSpecsDTO.Disks),
                Timestamp = systemSpecsDTO.Timestamp
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
