import {Box, IconButton, Paper, Popover, Stack} from "@mui/material";
import SystemPopover from "../SystemPopover/SystemPopover";
import GetHeaderColor from "../GetHeaderColor";
import MoreHorizIcon from "@mui/icons-material/MoreHoriz";
import Switch from "react-switch";
import PlayArrowIcon from "@mui/icons-material/PlayArrow";
import PauseIcon from "@mui/icons-material/Pause";
import {useState} from "react";
import {useCookies} from "react-cookie";
import {useTheme} from "@mui/material/styles";
import "./SystemControlCard.js.css"

export default function SystemControlCard({system, handleChangeAuthorisation, handleDeleteSystem, isDashboard}) {
    const [cookie] = useCookies(['systemAlias' + system.id])
    const [color, setColor] = useState(cookie['systemColor' + system.id] || "rgba(0, 0, 0, 0)")
    const [anchorEl, setAnchorEl] = useState(null)
    const theme = useTheme();

    return (
        <div>
            <Popover
                open={Boolean(anchorEl)}
                anchorEl={anchorEl}
                onClose={() => setAnchorEl(null)}
                anchorOrigin={{
                    vertical: 'top', horizontal: 'left',
                }}
                transformOrigin={{
                    vertical: 'top', horizontal: 'left',
                }}
            >
                <SystemPopover system={system} setAnchorEl={setAnchorEl} setColor={setColor}
                               handleDeleteSystem={() => handleDeleteSystem(system)} showNavigation={isDashboard}/>
            </Popover>
            <Box className={"system-control-card-header"} style={{backgroundColor: color}}>
                <Stack direction={"row"} justifyContent={"space-between"} alignItems={"center"}>
                    <IconButton onClick={(event) => setAnchorEl(event.currentTarget)}
                                style={{padding: 0, color: GetHeaderColor(color)}}>
                        <MoreHorizIcon/>
                    </IconButton>
                    <Paper sx={{height: "25px"}}>
                        <Switch
                            checked={system.isAuthorised}
                            onChange={() => handleChangeAuthorisation(system)}
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
                <div className={"system-info-card-title"} style={{color: GetHeaderColor(color)}}>
                    {cookie['systemAlias' + system.id]}
                </div>
            </Box>
        </div>
    );
}