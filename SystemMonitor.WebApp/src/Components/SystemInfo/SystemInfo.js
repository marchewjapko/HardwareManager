import {Box, Paper, Stack, Tab, Tabs,} from "@mui/material";
import "./SystemInfo.js.css"
import {useEffect, useState} from "react";
import UsageTab from "./Usage/UsageTab";
import SpeedIcon from '@mui/icons-material/Speed';
import InfoIcon from '@mui/icons-material/Info';
import SkeletonAccordions from "./Usage/SkeletonAccordions";
import SpecsTab from "./Specs/SpecsTab";

function GetSystemUsage({isLoading, reading}) {
    if (isLoading) {
        return (
            <SkeletonAccordions/>
        );
    }
    return (
        <UsageTab reading={reading}/>
    );
}

export default function SystemInfo({systemInfo}) {
    const [reading, setReading] = useState()
    const [isLoading, setIsLoading] = useState(true)
    const [value, setValue] = useState(0);
    const [error, setError] = useState()

    useEffect(() => {
        const interval = setInterval(() => {
            let url = "http://192.168.1.2:8080/GetSystem?"
            systemInfo.systemMacs.forEach(x => url += "ids=" + x.replaceAll(':', '%3A') + '&')
            url += "limit=1"
            fetch(url)
                .then(res => res.json())
                .then(
                    (result) => {
                        setIsLoading(false);
                        setReading(result.systemReadingDTOs[0]);
                    },
                    (error) => {
                        setIsLoading(false);
                        setError(error);
                    }
                )
        }, 2500);
        return () => clearInterval(interval);
    }, []);

    // useEffect(() => {
    //     let url = "https://localhost:7298/GetSystem?"
    //     systemInfo.systemMacs.forEach(x => url += "ids=" + x.replaceAll(':', '%3A') + '&')
    //     url += "limit=1"
    //     fetch(url)
    //         .then(res => res.json())
    //         .then(
    //             (result) => {
    //                 setIsLoading(false);
    //                 setReading(result.systemReadingDTOs[0]);
    //             },
    //             (error) => {
    //                 setIsLoading(false);
    //                 setError(error);
    //             }
    //         )
    // }, [])

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
                        <Tab icon={<SpeedIcon/>} label="Usage" iconPosition="start" disabled={isLoading}/>
                        <Tab icon={<InfoIcon/>} label="Details" iconPosition="end" disabled={isLoading}/>
                    </Tabs>
                </Box>
                {value === 0 ? (
                    <GetSystemUsage isLoading={isLoading} reading={reading}/>
                ) : (
                    <SpecsTab reading={reading}/>
                )}
            </Stack>
        </Paper>
    );
}