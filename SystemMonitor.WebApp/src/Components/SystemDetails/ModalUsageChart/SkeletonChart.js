import {CanvasJSChart} from "canvasjs-react-charts";
import {useTheme} from '@mui/material/styles'
import moment from "moment";
import {CircularProgress} from "@mui/material";

function GetOptions() {
    const theme = useTheme();
    return {
        zoomEnabled: true,
        backgroundColor: "rgba(0,0,0,0)",
        theme: theme.palette.mode === 'light' ? "light2" : "dark1",
        axisX: {
            minimum: 0,
            maximum: 100
        },
        axisY: {
            minimum: 0,
            maximum: 100
        },
        toolTip: {
            contentFormatter: function (e) {
                const date = moment(e.entries[0].dataPoint.x).format("DD.MM HH:mm")
                const usage = Math.round(e.entries[0].dataPoint.y * 10) / 10
                return date + ' - ' + usage + '%';
            }
        },
        data: [{
            type: "line",
            dataPoints: []
        }]
    }
}

export default function SkeletonChart() {
    return (
        <div className={"modal-usage-chart-skeleton-container"}>
            <div>
                <CanvasJSChart options={GetOptions()} className={"skeleton-usage-chart"}/>
            </div>
            <div className={"skeleton-progress-container"}>
                <CircularProgress className={"skeleton-progress"} size={"10em"}/>
            </div>
        </div>
    );
}