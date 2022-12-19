import {Paper, Typography} from "@mui/material";
import NetworkAccordion from "./NetworkAccordion";
import DiskAccordion from "./DiskAccordion";

export default function SpecsTab({reading, id}) {
    return (
        <div>
            <div className={"system-info-stack"}>
                <Paper square className={"system-info-row-text-only"}>
                    <div className={"system-info-row-title"}>
                        Operating system
                    </div>
                    <Typography variant={"body1"}>
                        {reading.systemSpecsDTO.osNameVersion}
                    </Typography>
                </Paper>
                <Paper square className={"system-info-row-text-only"}>
                    <div className={"system-info-row-title"}>
                        Processor
                    </div>
                    <Typography variant={"body1"}>
                        {reading.systemSpecsDTO.cpuInfo}
                    </Typography>
                    <Typography variant={"body1"}>
                        Cores: {reading.systemSpecsDTO.cpuCores}
                    </Typography>
                </Paper>
                <Paper square className={"system-info-row-text-only"}>
                    <div className={"system-info-row-title"}>
                        RAM
                    </div>
                    <Typography variant={"body1"}>
                        {Math.round(reading.systemSpecsDTO.totalMemory / 1024 / 1024 * 10) / 10} GB
                    </Typography>
                </Paper>
                <NetworkAccordion networkSpecs={reading.systemSpecsDTO.networkSpecs} id={id}/>
                <DiskAccordion diskSpecs={reading.systemSpecsDTO.diskSpecs} id={id}/>
            </div>
        </div>
    );
}