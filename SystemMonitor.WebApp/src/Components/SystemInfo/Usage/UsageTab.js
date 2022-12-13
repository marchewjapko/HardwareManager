import CpuAccordion from "./CpuAccordion";
import DiskAccordion from "./DiskAccordion";
import MemoryAccordion from "./MemoryAccordion";
import NetworkAccordion from "./NetworkAccordion";

export default function UsageTab({reading}) {
    return (
        <div className={"system-info-stack"}>
            <CpuAccordion cpuPerCoreUsage={reading.usageDTO.cpuPerCoreUsage}
                          cpuTotalUsage={reading.usageDTO.cpuTotalUsage}/>
            <DiskAccordion diskUsage={reading.usageDTO.diskUsage}/>
            <MemoryAccordion availabeMemory={reading.usageDTO.memoryUsage}
                             totalMemory={reading.systemSpecsDTO.totalMemory}/>
            <NetworkAccordion networkUsage={reading.usageDTO.networkUsage}
                              bandwidths={reading.systemSpecsDTO.networkSpecs}/>
        </div>
    );
}