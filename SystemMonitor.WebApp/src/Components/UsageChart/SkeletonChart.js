import {CanvasJSChart} from "canvasjs-react-charts";
import {useTheme} from '@mui/material/styles'
import moment from "moment";
import {CircularProgress} from "@mui/material";

function GetMockData() {
    let data = []
    var y = 100;
    for(let i = 0; i<200; i++) {
        y += Math.round(Math.random() * 10 - 5);
        data.push({
            x: i,
            y: y
        });
    }
}

function GetOptions() {
    const theme = useTheme();
    return {
        zoomEnabled: true,
        backgroundColor: "rgba(0,0,0,0)",
        theme: theme.palette.mode === 'light' ? "light2" : "dark1",
        animationEnabled: true,
        title: {
            text: "Total CPU usage",
            fontFamily: "Helvetica",
        },
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
            dataPoints: GetMockData()
        }]
    }
}

export default function SkeletonChart() {
    return (
        <div>
            <CanvasJSChart options={GetOptions()} className={"skeleton-usage-chart"}/>
            <CircularProgress className={"skeleton-progress"}/>
        </div>
    );
}