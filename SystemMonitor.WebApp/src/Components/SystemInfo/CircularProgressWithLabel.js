import {Box, CircularProgress, Typography} from "@mui/material";
import "./CircularProgressWithLabel.js.css"

export default function CircularProgressWithLabel(props) {
    let color = "success"
    if(props.value >= 25 && props.value < 80) {
        color = "primary"
    } else if(props.value >= 80 && props.value < 90) {
        color = "warning"
    } else if(props.value >= 90) {
        color = "error"
    }
    return (
        <div className={"circular-progress-with-label-container"}>
            <CircularProgress variant="determinate" {...props} color={color} />
            <div className={"circular-progress-with-label-label-container"}>
                <Typography variant="caption" component="div">
                    {Math.round(props.value)}%
                </Typography>
            </div>
        </div>
    );
}