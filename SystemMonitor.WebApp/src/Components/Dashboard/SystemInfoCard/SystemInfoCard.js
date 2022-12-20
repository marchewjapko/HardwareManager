import {Box, CircularProgress, IconButton, Paper, Tab, Tabs} from "@mui/material";
import "./SystemInfo.js.css"
import {useEffect, useState} from "react";
import UsageTab from "./Usage/UsageTab";
import SpeedIcon from '@mui/icons-material/Speed';
import InfoIcon from '@mui/icons-material/Info';
import SkeletonAccordions from "./Usage/Accordions/SkeletonAccordions";
import SpecsTab from "./Specs/SpecsTab";
import PauseIcon from '@mui/icons-material/Pause';
import {useCookies} from 'react-cookie';
import moment from "moment";
import SystemControlCard from "../../Shared/SystemControlCard/SystemControlCard";

export default function SystemInfoCard({system, handleChangeAuthorisation, handleDeleteSystem}) {
    const [currentTab, setCurrentTab] = useState(0)
    const [cookie, setCookie] = useCookies(['systemAlias' + system.id]);

    useEffect(() => {
        if (!cookie['systemAlias' + system.id]) {
            setCookie('systemAlias' + system.id, system.systemName, {
                path: '/',
                expires: new Date(2147483647 * 1000),
                sameSite: "lax"
            })
        }
        if (!cookie['systemColor' + system.id]) {
            setCookie('systemColor' + system.id, "rgba(0, 0, 0, 0)", {
                path: '/',
                expires: new Date(2147483647 * 1000),
                sameSite: "lax"
            })
        }
    }, []);

    const handleSwitchClick = () => {
        handleChangeAuthorisation(system)
    }

    function GetUsageTab() {
        if (!system.isAuthorised) {
            return (
                <div className={"skeleton-accordion-group-container"}>
                    <SkeletonAccordions/>
                    <IconButton className={"skeleton-accordion-group-spinner"}
                                onClick={handleSwitchClick}>
                        <PauseIcon fontSize={"large"}/>
                    </IconButton>
                </div>
            );
        }
        if (!system.systemReadingDTOs || system.systemReadingDTOs.length === 0) {
            return (
                <div className={"skeleton-accordion-group-container"}>
                    <SkeletonAccordions/>
                    <CircularProgress className={"skeleton-accordion-group-spinner"}/>
                </div>
            );
        }
        let timestamp = moment(system.systemReadingDTOs[0].timestamp).utc()
        if (moment.duration(moment().utc().diff(timestamp)).asMinutes() > 1) {
            return (
                <div className={"skeleton-accordion-group-container"}>
                    <SkeletonAccordions/>
                    <CircularProgress className={"skeleton-accordion-group-spinner"}/>
                </div>
            );
        }
        return (
            <UsageTab reading={system.systemReadingDTOs[0]} id={system.id}/>
        );
    }

    return (
        <Paper square={false} elevation={20} className={"system-info-card-container"}>
            <SystemControlCard system={system} handleDeleteSystem={handleDeleteSystem}
                               handleChangeAuthorisation={handleChangeAuthorisation} isDashboard={true}/>
            <div className={"system-info-card-body"}>
                <Box>
                    <Tabs value={currentTab} onChange={(event, newValue) => setCurrentTab(newValue)} variant="fullWidth"
                          className={"system-info-card-tab-container"}>
                        <Tab icon={<SpeedIcon/>} label="Usage" iconPosition="start"
                             disabled={!system.systemReadingDTOs || !system.isAuthorised}
                             className={"system-info-card-tab"}
                        />
                        <Tab icon={<InfoIcon/>} label="Details" iconPosition="end"
                             disabled={!system.systemReadingDTOs || !system.isAuthorised}
                             className={"system-info-card-tab"}/>
                    </Tabs>
                </Box>
                {(currentTab === 0 ? (
                    <GetUsageTab/>
                ) : (
                    <SpecsTab reading={system.systemReadingDTOs[0]} id={system.id}/>
                ))}
                <div className={"system-info-footer"}>
                    {system.systemReadingDTOs[0] && "Last reading: " + moment(system.systemReadingDTOs[0].timestamp).format("D.MM.YYYY HH:mm:ss")}
                </div>
            </div>
        </Paper>);
}