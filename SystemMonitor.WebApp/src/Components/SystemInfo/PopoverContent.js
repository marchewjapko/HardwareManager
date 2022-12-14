import {IconButton, Stack, TextField} from "@mui/material";
import RotateLeftIcon from "@mui/icons-material/RotateLeft";
import {GithubPicker} from "react-color";
import {useCookies} from "react-cookie";
import {useTheme} from '@mui/material/styles'
import "./PopoverContent.js.css"

export default function PopoverContent({systemInfo, setAnchorEl, setColor}) {
    const [cookies, setCookie] = useCookies(['systemAlias' + systemInfo.id]);
    const theme = useTheme();

    const handleChangeSystemAlias = (val) => {
        setCookie('systemAlias' + systemInfo.id, val.target.value, {path: '/', sameSite: "lax"})
    }

    const handleResetSystemAlias = () => {
        setCookie('systemAlias' + systemInfo.id, systemInfo.systemName, {path: '/', sameSite: "lax"})
        setAnchorEl(null)
    }

    const handleColorChange = (val) => {
        setColor(`rgba(${val.rgb.r}, ${val.rgb.g}, ${val.rgb.b}, ${val.rgb.a})`)
        setCookie('systemColor' + systemInfo.id, `rgba(${val.rgb.r}, ${val.rgb.g}, ${val.rgb.b}, ${val.rgb.a})`, {path: '/', sameSite: "lax"})
        setAnchorEl(null)
    }

    const handleResetColor = () => {
        setColor("rgba(0, 0, 0, 0)")
        setCookie('systemColor' + systemInfo.id, "rgba(0, 0, 0, 0)", {path: '/', sameSite: "lax"})
        setAnchorEl(null)
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
        </div>
    );
}