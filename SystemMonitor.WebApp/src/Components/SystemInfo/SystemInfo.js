import {Box, CircularProgress, IconButton, Paper, Popover, Stack, Tab, Tabs} from "@mui/material";
import "./SystemInfo.js.css"
import {useEffect, useState} from "react";
import UsageTab from "./Usage/UsageTab";
import SpeedIcon from '@mui/icons-material/Speed';
import InfoIcon from '@mui/icons-material/Info';
import SkeletonAccordions from "./Usage/Accordions/SkeletonAccordions";
import SpecsTab from "./Specs/SpecsTab";
import MoreHorizIcon from '@mui/icons-material/MoreHoriz';
import PauseIcon from '@mui/icons-material/Pause';
import PlayArrowIcon from '@mui/icons-material/PlayArrow';
import Switch from "react-switch";
import {useTheme} from '@mui/material/styles'
import {useCookies} from 'react-cookie';
import PopoverContent from "./PopoverContent";
import moment from "moment";

export default function SystemInfo({systemInfo, handleChangeAuthorisation, handleDeleteSystem}) {
    const [currentTab, setCurrentTab] = useState(0)
    const [anchorEl, setAnchorEl] = useState(null)
    const [cookies, setCookie] = useCookies(['systemAlias' + systemInfo.id]);
    const [color, setColor] = useState(cookies['systemColor' + systemInfo.id] || "rgba(0, 0, 0, 0)")
    const [isPaused, setIsPaused] = useState(!systemInfo.isAuthorised)
    const theme = useTheme();

    useEffect(() => {
        if (!cookies['systemAlias' + systemInfo.id]) {
            setCookie('systemAlias' + systemInfo.id, systemInfo.systemName, {path: '/', sameSite: "lax"})
        }
        if (!cookies['systemColor' + systemInfo.id]) {
            setCookie('systemColor' + systemInfo.id, "rgba(0, 0, 0, 0)", {path: '/', sameSite: "lax"})
        }
    }, []);

    function getHeaderFontColor() {
        let colorParse = color.replaceAll("rgba(", '')
        colorParse = colorParse.replaceAll(")", '').replaceAll(" ", '')
        const newColor = {
            r: parseInt(colorParse[0]),
            g: parseInt(colorParse[1]),
            b: parseInt(colorParse[2]),
            a: parseInt(colorParse[3])
        }
        if (newColor.r === 0 && newColor.g === 0 && newColor.b === 0 && newColor.a === 0) {
            if (theme.palette.mode === 'dark') {
                return "white"
            } else {
                return "black"
            }
        }
        if (newColor.r * 0.299 + newColor.g * 0.587 + newColor.b * 0.144 < 186) {
            return "black"
        } else {
            return "white"
        }
    }

    const handleSwitchClick = () => {
        setIsPaused(!isPaused)
        handleChangeAuthorisation(systemInfo)
    }

    function GetUsageTab() {
        if (!systemInfo.isAuthorised) {
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
        if (!systemInfo.systemReadingDTOs || systemInfo.systemReadingDTOs.length === 0) {
            return (
                <div className={"skeleton-accordion-group-container"}>
                    <SkeletonAccordions/>
                    <CircularProgress className={"skeleton-accordion-group-spinner"}/>
                </div>
            );
        }
        let timestamp = moment(systemInfo.systemReadingDTOs[0].timestamp).utc()
        if(moment.duration(moment().utc().diff(timestamp)).asMinutes() > 1) {
            return (
                <div className={"skeleton-accordion-group-container"}>
                    <SkeletonAccordions/>
                    <CircularProgress className={"skeleton-accordion-group-spinner"}/>
                </div>
            );
        }
        return (
            <UsageTab reading={systemInfo.systemReadingDTOs[0]} id={systemInfo.id}/>
        );
    }

    return (<Paper square={false} elevation={20} className={"system-info-card-container"}>
        <Popover
            open={Boolean(anchorEl)}
            anchorEl={anchorEl}
            onClose={() => setAnchorEl(null)}
            anchorOrigin={{
                vertical: 'top', horizontal: 'right',
            }}
            transformOrigin={{
                vertical: 'bottom', horizontal: 'left',
            }}
        >
            <PopoverContent systemInfo={systemInfo} setAnchorEl={setAnchorEl} setColor={setColor} handleDeleteSystem={handleDeleteSystem}/>
        </Popover>
        <Box className={"system-info-card-header"} style={{backgroundColor: color}}>
            <Stack direction={"row"} justifyContent={"space-between"} alignItems={"center"}>
                <IconButton onClick={(event) => setAnchorEl(event.currentTarget)}
                            style={{padding: 0, color: getHeaderFontColor()}}>
                    <MoreHorizIcon/>
                </IconButton>
                <Paper sx={{height: "25px"}}>
                    <Switch
                        checked={!isPaused}
                        onChange={handleSwitchClick}
                        handleDiameter={20}
                        offColor={theme.palette.background.paper}
                        onColor={theme.palette.background.paper}
                        offHandleColor={theme.palette.primary.main}
                        onHandleColor={theme.palette.success.light}
                        height={25}
                        width={50}
                        borderRadius={4}
                        activeBoxShadow="0px 0px 1px 2px #fffc35"
                        uncheckedIcon={<PlayArrowIcon className={"switch-icon"}/>}
                        checkedIcon={<PauseIcon className={"switch-icon"}/>}
                        uncheckedHandleIcon={<PauseIcon className={"switch-handle-icon"}/>}
                        checkedHandleIcon={<PlayArrowIcon className={"switch-handle-icon"}/>}
                    />
                </Paper>
            </Stack>
            <div className={"system-info-card-title"} style={{color: getHeaderFontColor()}}>
                {cookies['systemAlias' + systemInfo.id]}
            </div>
        </Box>
        <div className={"system-info-card-body"}>
            <Box>
                <Tabs value={currentTab} onChange={(event, newValue) => setCurrentTab(newValue)} variant="fullWidth"
                      className={"system-info-card-tab-container"}>
                    <Tab icon={<SpeedIcon/>} label="Usage" iconPosition="start"
                         disabled={!systemInfo.systemReadingDTOs || !systemInfo.isAuthorised}
                         className={"system-info-card-tab"}
                    />
                    <Tab icon={<InfoIcon/>} label="Details" iconPosition="end"
                         disabled={!systemInfo.systemReadingDTOs || !systemInfo.isAuthorised}
                         className={"system-info-card-tab"}/>
                </Tabs>
            </Box>
            {(currentTab === 0 ? (
                <GetUsageTab/>
            ) : (
                <SpecsTab reading={systemInfo.systemReadingDTOs[0]} id={systemInfo.id}/>
            ))}
            <div className={"system-info-footer"}>
                {systemInfo.systemReadingDTOs[0] && "Last reading: " + moment(systemInfo.systemReadingDTOs[0].timestamp).format("D.MM.YYYY HH:mm:ss")}
            </div>
        </div>
    </Paper>);
}