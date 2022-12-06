import {Paper, Typography} from "@mui/material";
import NetworkAccordion from "./NetworkAccordion";
import DiskAccordion from "./DiskAccordion";

export default function SpecsTab({systemInfo}) {
    return (
        <div>
            <Paper>
                <Typography variant="h5">
                    Operating system
                </Typography>
                <Typography variant={"body1"}>
                    {systemInfo.systemReadingDTOs[0].systemSpecsDTO.osNameVersion}
                </Typography>
            </Paper>
            <Paper>
                <Typography variant="h5">
                    Processor
                </Typography>
                <Typography variant={"body1"}>
                    {systemInfo.systemReadingDTOs[0].systemSpecsDTO.cpuInfo}
                </Typography>
                <Typography variant={"body1"}>
                    Cores: {systemInfo.systemReadingDTOs[0].systemSpecsDTO.cpuCores}
                </Typography>
            </Paper>
            <Paper>
                <Typography variant="h5">
                    RAM
                </Typography>
                <Typography variant={"body1"}>
                    {Math.round(systemInfo.systemReadingDTOs[0].systemSpecsDTO.totalMemory / 1024 / 1024 * 10) / 10} GB
                </Typography>
            </Paper>
            <NetworkAccordion networkAdapters={systemInfo.systemReadingDTOs[0].systemSpecsDTO.networkAdapters}/>
            <DiskAccordion disks={systemInfo.systemReadingDTOs[0].systemSpecsDTO.disks}/>
        </div>);
}