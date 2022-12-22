import {useTheme} from "@mui/material/styles";
import moment from "moment";
import {
    Backdrop,
    CircularProgress,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow
} from "@mui/material";
import {CanvasJSChart} from "canvasjs-react-charts";

export default function MemoryDetailsSkeleton() {
    const theme = useTheme()
    function GetOptions() {
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
                    valueFormatString: "DD-MM HH:mm",
                    minimum: 0,
                    maximum: 100
                },
                axisY: {
                    valueFormatString: "##.##'%'",
                    minimum: 0,
                    maximum: 100
                },
                toolTip: {
                    contentFormatter: function (e) {
                        const date = moment(e.entries[0].dataPoint.x).format("DD.MM HH:mm:ss")
                        const usage = Math.round(e.entries[0].dataPoint.y * 10) / 10
                        return date + ' - ' + usage + '%';
                    }
                },
                data: []
            }
    }

    return (
        <Paper className={"system-details-card-container system-details-skeleton-card-container"} elevation={3}>
            <div className={"skeleton-overlay"}/>
            <div className={"system-details-skeleton-spinner"}>
                <CircularProgress color="inherit" size={"10em"}/>
            </div>
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
                                    -
                                </TableCell>
                            </TableRow>
                            <TableRow
                                key={1}
                            >
                                <TableCell component="th" scope="row">
                                    Used memory
                                </TableCell>
                                <TableCell align="right">
                                    -
                                </TableCell>
                            </TableRow>
                            <TableRow
                                key={2}
                            >
                                <TableCell component="th" scope="row">
                                    Available memory
                                </TableCell>
                                <TableCell align="right">
                                    -
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