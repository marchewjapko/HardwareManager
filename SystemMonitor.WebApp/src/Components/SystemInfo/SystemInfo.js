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

export default function SystemInfo({systemInfo, setSystems, systems}) {
    const [system, setSystem] = useState(systemInfo)
    const [reading, setReading] = useState()
    const [currentTab, setCurrentTab] = useState(0)
    const [error, setError] = useState()
    const [anchorEl, setAnchorEl] = useState(null)
    const [cookies, setCookie] = useCookies(['systemAlias' + system.id]);
    const [color, setColor] = useState(cookies['systemColor' + system.id] || "rgba(0, 0, 0, 0)")
    const theme = useTheme();

    function getReading() {
        let url = "http://192.168.1.2:8080/GetSystem?"
        system.systemMacs.forEach(x => url += "ids=" + x.replaceAll(':', '%3A') + '&')
        url += "limit=1"
        fetch(url)
            .then(res => res.json())
            .then((result) => {
                if(result.status && result.status === 404) {
                    setSystems(systems.filter((x) => x.id !== systemInfo.id))
                }
                let newSystem = {
                    id: result.id,
                    isAuthorised: result.isAuthorised,
                    systemMacs: result.systemMacs,
                    systemName: result.systemName
                }
                setSystem(newSystem)
                if (newSystem.isAuthorised) {
                    setReading(result.systemReadingDTOs[0]);
                }
            }, (error) => {
                setError(error);
            })
    }

    useEffect(() => {
        getReading()
        const interval = setInterval(() => {
            getReading()
        }, 2500);
        return () => clearInterval(interval);
    }, [system.isAuthorised]);

    useEffect(() => {
        if (!cookies['systemAlias' + system.id]) {
            setCookie('systemAlias' + system.id, system.systemName, {path: '/', sameSite: "lax"})
        }
        if (!cookies['systemColor' + system.id]) {
            setCookie('systemColor' + system.id, theme.palette.background.paper, {path: '/', sameSite: "lax"})
        }
    }, []);

    function getHeaderFontColor() {
        let colorParse = color.replaceAll("rgba(", '')
        colorParse = colorParse.replaceAll(")", '')
        colorParse = colorParse.replaceAll(" ", '')
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

    const handleChangeAuthorisation = async () => {
        const requestOptions = {
            method: 'PUT', headers: {'Content-Type': 'application/json'}, body: JSON.stringify({
                id: system.id,
                isAuthorised: !system.isAuthorised,
                systemMacs: system.systemMacs,
                systemName: system.systemName
            })
        };
        const response = await fetch('http://192.168.1.2:8080/UpdateSystem?id=' + system.id, requestOptions);
        if (response.ok) {
            setSystem({...system, isAuthorised: !system.isAuthorised})
        }
    }

    function GetUsageTab() {
        if (!system.isAuthorised) {
            return (
                <div className={"skeleton-accordion-group-container"}>
                    <SkeletonAccordions/>
                    <IconButton className={"skeleton-accordion-group-spinner"} onClick={handleChangeAuthorisation}>
                        <PauseIcon fontSize={"large"}/>
                    </IconButton>
                </div>
            );
        }
        if (!reading) {
            return (
                <div className={"skeleton-accordion-group-container"}>
                    <SkeletonAccordions/>
                    <CircularProgress className={"skeleton-accordion-group-spinner"}/>
                </div>
            );
        }
        return (
            <UsageTab reading={reading} id={system.id}/>
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
            <PopoverContent systemInfo={system} setAnchorEl={setAnchorEl} setColor={setColor}/>
        </Popover>
        <Box className={"system-info-card-header"} style={{backgroundColor: color}}>
            <Stack direction={"row"} justifyContent={"space-between"} alignItems={"center"}>
                <IconButton onClick={(event) => setAnchorEl(event.currentTarget)}
                            style={{padding: 0, color: getHeaderFontColor()}}>
                    <MoreHorizIcon/>
                </IconButton>
                <Paper sx={{height: "25px"}}>
                    <Switch
                        checked={system.isAuthorised}
                        onChange={handleChangeAuthorisation}
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
                {cookies['systemAlias' + system.id]}
            </div>
        </Box>
        <div className={"system-info-card-body"}>
            <Box>
                <Tabs value={currentTab} onChange={(event, newValue) => setCurrentTab(newValue)} variant="fullWidth"
                      className={"system-info-card-tab-container"}>
                    <Tab icon={<SpeedIcon/>} label="Usage" iconPosition="start"
                         disabled={!reading || !system.isAuthorised}
                         className={"system-info-card-tab"}
                    />
                    <Tab icon={<InfoIcon/>} label="Details" iconPosition="end"
                         disabled={!reading || !system.isAuthorised}
                         className={"system-info-card-tab"}/>
                </Tabs>
            </Box>
            {(currentTab === 0 ? (
                <GetUsageTab/>
            ) : (
                <SpecsTab reading={reading} id={system.id}/>
            ))}
        </div>
    </Paper>);
}