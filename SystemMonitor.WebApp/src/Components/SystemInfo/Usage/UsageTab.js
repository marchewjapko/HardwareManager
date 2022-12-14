import CpuAccordion from "./Accordions/CpuAccordion";
import DiskAccordion from "./Accordions/DiskAccordion";
import MemoryAccordion from "./Accordions/MemoryAccordion";
import NetworkAccordion from "./Accordions/NetworkAccordion";

export default function UsageTab({reading, id}) {
    return (
        <div className={"system-info-stack"}>
            <CpuAccordion cpuPerCoreUsage={reading.usageDTO.cpuPerCoreUsage}
                          cpuTotalUsage={reading.usageDTO.cpuTotalUsage} id={id}/>
            <DiskAccordion diskUsage={reading.usageDTO.diskUsage} id={id}/>
            <MemoryAccordion availabeMemory={reading.usageDTO.memoryUsage}
                             totalMemory={reading.systemSpecsDTO.totalMemory} id={id}/>
            <NetworkAccordion networkUsage={reading.usageDTO.networkUsage}
                              bandwidths={reading.systemSpecsDTO.networkSpecs} id={id}/>
        </div>
    );
}