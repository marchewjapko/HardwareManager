import {useTheme} from "@mui/material/styles";
import moment from "moment";
import {CanvasJSChart} from "canvasjs-react-charts";
import {Button, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@mui/material";
import InsertChartIcon from "@mui/icons-material/InsertChart";

export default function MemoryDetails({dataPoints, usedMemory, specs, setIsDialogOpen, setUsageModalMetric}) {
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
                data: dataPoints
            }
        }
    }

    const handleOpenChartClick = () => {
        setUsageModalMetric('memory')
        setIsDialogOpen(true)
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
                <Stack direction={"row"} justifyContent={"flex-end"} marginBottom={"10px"}>
                    <Button variant="contained" endIcon={<InsertChartIcon/>} size="medium"
                            onClick={handleOpenChartClick}>
                        All readings
                    </Button>
                </Stack>
                <CanvasJSChart options={GetOptions()} className={"usage-chart"}/>
            </div>
        </Paper>
    );
}