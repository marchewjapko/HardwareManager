import Switch from "react-switch";
import {useTheme} from "@mui/material/styles";
import {useEffect, useState} from "react";
import WbSunnyIcon from '@mui/icons-material/WbSunny';
import NightlightIcon from '@mui/icons-material/Nightlight';
import "./Header.js.css"
import {
    CircularProgress,
    Divider,
    Drawer,
    IconButton,
    List,
    ListItem,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Paper
} from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import DashboardIcon from '@mui/icons-material/Dashboard';
import {useNavigate} from "react-router-dom";
import ComputerIcon from '@mui/icons-material/Computer';

export default function Header({connection, handleChangeTheme}) {
    const theme = useTheme();
    const [isDrawerOpen, setIsDrawerOpen] = useState(false)
    const [systems, setSystems] = useState()
    const navigate = useNavigate()

    const handleChange = () => {
        handleChangeTheme()
    }

    useEffect(() => {
        if (connection && connection.state !== 'Disconnected') {
            connection.on('ReceiveAllSystems', response => {
                if(!systems || response.length !== systems.length)
                    setSystems(response)
            })
            connection.send("BrowseAllSystems", 0)
        }
        return (() => {
            connection.off("ReceiveAllSystemHeader")
        })
    }, []);

    function GetSystemsList() {
        if (!systems) {
            return (
                <div className={"drawer-systems-list-loading"}>
                    <CircularProgress size={'3em'}/>
                </div>
            );
        }
        if (systems.length === 0) {
            return (
                <div className={"drawer-systems-list-no-systems"}>
                    No systems
                </div>
            );
        }
        return (
            <div>
                {systems.map((system) => (
                    <ListItem key={system.id} disablePadding>
                        <ListItemButton onClick={() => handleNavigate('system/' + system.id)}>
                            <ListItemIcon>
                                <ComputerIcon/>
                            </ListItemIcon>
                            <ListItemText primary={system.systemName}/>
                        </ListItemButton>
                    </ListItem>
                ))}
            </div>
        );
    }

    const handleNavigate = (url) => {
        setIsDrawerOpen(false)
        navigate(url)
    }

    return (
        <div>
            <Paper className={"header-container"} variant={"elevation"} square={true}>
                <div>
                    <IconButton
                        color="inherit"
                        onClick={() => setIsDrawerOpen(true)}
                        edge="start">
                        <MenuIcon/>
                    </IconButton>
                </div>
                <div>
                    <Switch
                        checked={theme.palette.mode === 'dark'}
                        onChange={handleChange}
                        handleDiameter={30}
                        onColor={theme.palette.grey["700"]}
                        onHandleColor={theme.palette.grey["900"]}
                        offColor={theme.palette.grey["400"]}
                        offHandleColor={theme.palette.common.white}
                        height={38}
                        width={80}
                        boxShadow="0px 2px 2px rgba(0, 0, 0, 0.5)"
                        activeBoxShadow="0px 2px 2px rgba(0, 0, 0, 0.5)"
                        uncheckedIcon={
                            <div className={"header-unchecked-icon-container"}>
                                <WbSunnyIcon className={"header-icon"}/>
                            </div>
                        }
                        checkedIcon={
                            <div className={"header-checked-icon-container"}>
                                <NightlightIcon className={"header-icon"}/>
                            </div>
                        }
                    />
                </div>
            </Paper>
            <Drawer
                variant="persistent"
                anchor="left"
                open={isDrawerOpen}
            >
                <Paper className={"drawer-paper"} variant={"elevation"} square={true}>
                    <div className={"drawer-header"}>
                        <div className={"drawer-title"}>
                            System monitor
                        </div>
                        <IconButton className={"drawer-header-icon-container"} onClick={() => setIsDrawerOpen(false)}>
                            <ChevronLeftIcon fontSize={"large"}/>
                        </IconButton>
                    </div>
                    <Divider/>
                    <List>
                        <ListItem key={'mainPageButton'} disablePadding>
                            <ListItemButton onClick={() => handleNavigate('/')}>
                                <ListItemIcon>
                                    <DashboardIcon/>
                                </ListItemIcon>
                                <ListItemText primary={"Main page"}/>
                            </ListItemButton>
                        </ListItem>
                        <Divider/>
                        <GetSystemsList/>
                    </List>
                </Paper>
            </Drawer>
        </div>
    );
}