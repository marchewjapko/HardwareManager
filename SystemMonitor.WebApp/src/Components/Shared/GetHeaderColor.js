import {useTheme} from "@mui/material/styles";

export default function GetHeaderColor(color) {
    const theme = useTheme();
    let colorParse = color.split(',').map((x) => {
        return x.replace(/\D/g, "")
    });
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
    if (newColor.r * 0.299 + newColor.g * 0.587 + newColor.b * 0.144 > 186) {
        return "black"
    } else {
        return "white"
    }
}