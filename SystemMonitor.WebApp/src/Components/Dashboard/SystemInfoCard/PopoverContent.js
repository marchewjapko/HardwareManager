import {Button, IconButton, Stack, TextField} from "@mui/material";
import RotateLeftIcon from "@mui/icons-material/RotateLeft";
import {GithubPicker} from "react-color";
import {useCookies} from "react-cookie";
import "./PopoverContent.js.css"
import DeleteIcon from '@mui/icons-material/Delete';
import InsertChartIcon from '@mui/icons-material/InsertChart';
import {useNavigate} from "react-router-dom";

export default function PopoverContent({systemInfo, setAnchorEl, setColor, handleDeleteSystem}) {
    const [cookies, setCookie] = useCookies(['systemAlias' + systemInfo.id])
    const navigate = useNavigate();

    const handleChangeSystemAlias = (val) => {
        setCookie('systemAlias' + systemInfo.id, val.target.value, {path: '/', sameSite: "lax"})
    }

    const handleResetSystemAlias = () => {
        setCookie('systemAlias' + systemInfo.id, systemInfo.systemName, {path: '/', sameSite: "lax"})
    }

    const handleColorChange = (val) => {
        setColor(`rgba(${val.rgb.r}, ${val.rgb.g}, ${val.rgb.b}, ${val.rgb.a})`)
        setCookie('systemColor' + systemInfo.id, `rgba(${val.rgb.r}, ${val.rgb.g}, ${val.rgb.b}, ${val.rgb.a})`, {
            path: '/',
            sameSite: "lax"
        })
    }

    const handleResetColor = () => {
        setColor("rgba(0, 0, 0, 0)")
        setCookie('systemColor' + systemInfo.id, "rgba(0, 0, 0, 0)", {path: '/', sameSite: "lax"})
    }

    const handleDeleteClick = () => {
        setAnchorEl(null)
        handleDeleteSystem(systemInfo)
    }

    return (
        <div className={"system-info-popover"}>
            <Stack direction={"row"} className={"system-info-popover-input"}>
                <TextField value={cookies['systemAlias' + systemInfo.id]}
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
                <Button variant="contained" color="primary" className={"system-info-popover-button"}
                        startIcon={<InsertChartIcon fontSize={"small"} size={"small"}/>} fullWidth size={"small"}
                        onClick={() => navigate('/system/' + systemInfo.id)}>
                    View chart
                </Button>
                <Button variant="outlined" color="error" className={"system-info-popover-button"}
                        startIcon={<DeleteIcon fontSize={"small"} size={"small"}/>} onClick={handleDeleteClick}
                        fullWidth size={"small"}>
                    Delete
                </Button>
            </div>
        </div>
    );
}