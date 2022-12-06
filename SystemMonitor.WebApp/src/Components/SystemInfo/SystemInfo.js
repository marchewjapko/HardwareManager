import {Box, Paper, Stack, Tab, Tabs,} from "@mui/material";
import "./SystemInfo.js.css"
import {SystemInfoMock} from "../../Mocks/SystemInfoMock";
import {useState} from "react";
import UsageTab from "./Usage/UsageTab";
import SpeedIcon from '@mui/icons-material/Speed';
import InfoIcon from '@mui/icons-material/Info';
import SkeletonAccordions from "./Usage/SkeletonAccordions";
import SpecsTab from "./Specs/SpecsTab";

function GetSystemUsage({isLoading, systemInfo}) {
    if(isLoading) {
        return (
            <SkeletonAccordions/>
        );
    }
    return (
        <UsageTab systemInfo={systemInfo}/>
    );
}

export default function SystemInfo() {
    const [systemInfo, setSystemInfo] = useState(SystemInfoMock[0])
    const [isLoading, setIsLoading] = useState(false)
    const [value, setValue] = useState(0);
    const handleChange = (event, newValue) => {
        setValue(newValue);
    };
    return (
        <Paper square={false} elevation={20} className={"system-info-card"}>
            <Stack spacing={2}>
                <div className={"system-info-card-title"}>
                    {isLoading ? "Loading..." : systemInfo.systemName}
                </div>
                <Box sx={{borderBottom: 1, borderColor: 'divider'}}>
                    <Tabs value={value} onChange={handleChange} variant="fullWidth">
                        <Tab icon={<SpeedIcon/>} label="Usage" iconPosition="start"/>
                        <Tab icon={<InfoIcon/>} label="Details" iconPosition="end"/>
                    </Tabs>
                </Box>
                {value === 0 ? (
                    <GetSystemUsage isLoading={!systemInfo} systemInfo={systemInfo}/>
                ) : (
                    <SpecsTab systemInfo={systemInfo}/>
                )}
            </Stack>
        </Paper>
    );
}