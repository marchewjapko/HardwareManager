import {useTheme} from "@mui/material/styles";
import moment from "moment";
import {CanvasJSChart} from "canvasjs-react-charts";
import {Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@mui/material";

export default function MemoryDetails({dataPoints, usedMemory, specs}) {
    const theme = useTheme()
    function GetOptions() {
        if (dataPoints.length !== 0) {
            return {
                zoomEnabled: true,
                backgroundColor: "rgba(0,0,0,0)",
                theme: theme.palette.mode === 'light' ? "light2" : "dark1",
                animationEnabled: true,
                title: {
                    text: "Memory usage",
                    fontFamily: "Helvetica",
                    fontSize: 20,
                },
                axisX: {
                    valueFormatString: "DD-MM HH:mm"
                },
                axisY: {
                    valueFormatString: "##.##'%'",
                    minimum: 0,
                },
                toolTip: {
                    contentFormatter: function (e) {
                        const date = moment(e.entries[0].dataPoint.x).format("DD.MM HH:mm:ss")
                        const usage = Math.round(e.entries[0].dataPoint.y * 10) / 10
                        return date + ' - ' + usage + '%';
                    }
                },
                data: [{
                    type: "line",
                    lineThickness: 3,
                    lineColor: theme.palette.primary.main,
                    connectNullData: true,
                    dataPoints: dataPoints
                }]
            }
        }
    }

    return (
        <Paper className={"system-details-card-container"} elevation={3}>
            <div className={"system-details-card-title"}>
                Memory
            </div>
            <div className={"system-details-card-info"}>
                <TableContainer>
                    <Table size="small">
                        <TableHead>
                            <TableRow>
                                <TableCell></TableCell>
                                <TableCell align="right"></TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            <TableRow
                                key={0}
                            >
                                <TableCell component="th" scope="row">
                                    Total memory
                                </TableCell>
                                <TableCell align="right">
                                    {Math.round(specs.totalMemory / 1024 / 1024 * 10) / 10} GB
                                </TableCell>
                            </TableRow>
                            <TableRow
                                key={1}
                            >
                                <TableCell component="th" scope="row">
                                    Used memory
                                </TableCell>
                                <TableCell align="right">
                                    {Math.round((specs.totalMemory / 1024 / 1024 - usedMemory / 1024) * 10) / 10} GB
                                </TableCell>
                            </TableRow>
                            <TableRow
                                key={2}
                            >
                                <TableCell component="th" scope="row">
                                    Available memory
                                </TableCell>
                                <TableCell align="right">
                                    {Math.round((usedMemory / 1024) * 10) / 10} GB
                                </TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </TableContainer>
            </div>
            <div>
                <CanvasJSChart options={GetOptions()} className={"usage-chart"}/>
            </div>
        </Paper>
    );
}