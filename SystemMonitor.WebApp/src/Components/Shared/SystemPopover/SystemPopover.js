import {useNavigate} from "react-router-dom";
import {Button, Dialog, DialogTitle, IconButton, Stack, TextField} from "@mui/material";
import RotateLeftIcon from "@mui/icons-material/RotateLeft";
import {GithubPicker} from "react-color";
import InsertChartIcon from "@mui/icons-material/InsertChart";
import DeleteIcon from "@mui/icons-material/Delete";
import "./PopoverContent.js.css"
import ModalUsageChart from "../../SystemDetails/ModalUsageChart/ModalUsageChart";

export default function SystemPopover({
                                          system,
                                          setAnchorEl,
                                          setColor,
                                          handleDeleteSystem,
                                          showNavigation,
                                          cookie,
                                          setCookie
                                      }) {
    const navigate = useNavigate();

    const handleChangeSystemAlias = (val) => {
        setCookie('systemAlias' + system.id, val.target.value, {
            path: '/',
            expires: new Date(2147483647 * 1000),
            sameSite: "lax"
        })
    }

    const handleResetSystemAlias = () => {
        setCookie('systemAlias' + system.id, system.systemName, {
            path: '/',
            expires: new Date(2147483647 * 1000),
            sameSite: "lax"
        })
    }

    const handleColorChange = (val) => {
        setColor(`rgba(${val.rgb.r}, ${val.rgb.g}, ${val.rgb.b}, ${val.rgb.a})`)
        setCookie('systemColor' + system.id, `rgba(${val.rgb.r}, ${val.rgb.g}, ${val.rgb.b}, ${val.rgb.a})`, {
            path: '/',
            expires: new Date(2147483647 * 1000),
            sameSite: "lax"
        })
    }

    const handleResetColor = () => {
        setColor("rgba(0, 0, 0, 0)")
        setCookie('systemColor' + system.id, "rgba(0, 0, 0, 0)", {
            path: '/',
            expires: new Date(2147483647 * 1000),
            sameSite: "lax"
        })
    }

    const handleDeleteClick = () => {
        setAnchorEl(null)
        handleDeleteSystem(system)
    }

    return (
        <div className={"system-info-popover"}>
            <Stack direction={"row"} className={"system-info-popover-input"}>
                <TextField value={cookie['systemAlias' + system.id]}
                           onChange={handleChangeSystemAlias}
                           label={"System alias"}
                           margin={"none"}
                           size={"small"}
                           fullWidth
                />
                <IconButton className={"system-info-reset"} onClick={handleResetSystemAlias}>
                    <RotateLeftIcon/>
                </IconButton>
            </Stack>
            <Stack direction={"row"} className={"system-info-popover-input"}>
                <GithubPicker triangle={"hide"} width={"212px"} onChange={handleColorChange}/>
                <IconButton className={"system-info-reset"} onClick={handleResetColor}>
                    <RotateLeftIcon/>
                </IconButton>
            </Stack>
            <div className={"system-info-popover-button-group"}>
                {showNavigation &&
                    <Button variant="contained" color="primary" className={"system-info-popover-button"}
                            startIcon={<InsertChartIcon fontSize={"small"} size={"small"}/>} fullWidth size={"small"}
                            onClick={() => navigate('/system/' + system.id)}>
                        Details
                    </Button>
                }
                <Button variant="outlined" color="error" className={"system-info-popover-button"}
                        startIcon={<DeleteIcon fontSize={"small"} size={"small"}/>} onClick={handleDeleteClick}
                        fullWidth size={"small"}>
                    Delete
                </Button>
            </div>
        </div>
    );
}