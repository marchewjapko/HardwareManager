import {useTheme} from "@mui/material/styles";
import moment from "moment";
import {Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@mui/material";
import {CanvasJSChart} from "canvasjs-react-charts";

export default function DiskDetailsSkeleton() {
    const theme = useTheme();

    function GetOptions() {
        return {
            zoomEnabled: true,
            backgroundColor: "rgba(0,0,0,0)",
            theme: theme.palette.mode === 'light' ? "light2" : "dark1",
            animationEnabled: true,
            title: {
                text: "Total CPU usage",
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
            data: []
        }
    }

    return (
        <Paper className={"system-details-card-container"} elevation={3}>
            <div className={"system-details-card-title"}>
                Disks
            </div>
            <div className={"system-details-card-info"}>
                <TableContainer>
                    <Table size="small">
                        <TableHead>
                            <TableRow>
                                <TableCell>Name</TableCell>
                                <TableCell align="right">Size</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            <TableRow
                                sx={{'&:last-child td, &:last-child th': {border: 0}}}
                            >
                                <TableCell component="th" scope="row">
                                    -
                                </TableCell>
                                <TableCell align="right">
                                    - GB
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