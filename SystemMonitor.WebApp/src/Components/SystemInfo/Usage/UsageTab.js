import CpuAccordion from "./CpuAccordion";
import DiskAccordion from "./DiskAccordion";
import MemoryAccordion from "./MemoryAccordion";
import NetworkAccordion from "./NetworkAccordion";

export default function UsageTab({systemInfo}) {
    return (
        <div>
            <CpuAccordion cpuPerCoreUsage={systemInfo.systemReadingDTOs[0].usageDTO.cpuPerCoreUsage}
                          cpuTotalUsage={systemInfo.systemReadingDTOs[0].usageDTO.cpuTotalUsage}/>
            <DiskAccordion diskUsage={systemInfo.systemReadingDTOs[0].usageDTO.diskUsage}/>
            <MemoryAccordion memoryUsage={systemInfo.systemReadingDTOs[0].usageDTO.memoryUsage}
                             totalMemory={systemInfo.systemReadingDTOs[0].systemSpecsDTO.totalMemory}/>
            <NetworkAccordion bytesSent={systemInfo.systemReadingDTOs[0].usageDTO.bytesSent}
                              bytesReceived={systemInfo.systemReadingDTOs[0].usageDTO.bytesReceived}
                              networkAdapters={systemInfo.systemReadingDTOs[0].systemSpecsDTO.networkAdapters}/>
        </div>
    );
}