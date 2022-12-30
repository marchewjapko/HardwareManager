import {useTheme} from "@mui/material/styles";
import moment from "moment";
import {
    Button,
    FormControlLabel,
    Paper,
    Stack,
    Switch,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow
} from "@mui/material";
import {CanvasJSChart} from "canvasjs-react-charts";
import {useState} from "react";
import InsertChartIcon from "@mui/icons-material/InsertChart";

export default function NetworkDetails({dataPoints, specs, setIsDialogOpen, setUsageModalMetric}) {
    const [showPercentage, setShowPercentage] = useState(false)
    const theme = useTheme();

    function GetOptions() {
        if (dataPoints.length !== 0) {
            return {
                zoomEnabled: true,
                backgroundColor: "rgba(0,0,0,0)",
                theme: theme.palette.mode === 'light' ? "light2" : "dark1",
                animationEnabled: true,
                title: {
                    text: "Network usage",
                    fontFamily: "Helvetica",
                    fontSize: 20,
                },
                axisX: {
                    valueFormatString: "DD-MM HH:mm"
                },
                axisY: {
                    valueFormatString: showPercentage ? "##.##'%'" : "##.##kB/s",
                    minimum: 0,
                },
                toolTip: {
                    contentFormatter: function (e) {
                        const date = moment(e.entries[0].dataPoint.x).format("DD.MM HH:mm:ss")
                        const usage = Math.round(e.entries[0].dataPoint.y * 10) / 10
                        if (showPercentage) {
                            return date + ' - ' + usage + ' %'
                        }
                        return date + ' - ' + usage + ' KB/s';
                    }
                },
                data: showPercentage ? dataPoints.filter((x) => !x.name.includes('sent') && !x.name.includes('received')) : dataPoints.filter((x) => x.name.includes('sent') || x.name.includes('received'))
            }
        }
    }

    const handleOpenChartClick = () => {
        setUsageModalMetric('network')
        setIsDialogOpen(true)
    }

    return (
        <Paper className={"system-details-card-container"} elevation={3}>
            <div className={"system-details-card-title"}>
                Network
            </div>
            <div className={"system-details-card-info"}>
                <TableContainer>
                    <Table size="small">
                        <TableHead>
                            <TableRow>
                                <TableCell>Name</TableCell>
                                <TableCell align="right">Bandwidth</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {specs.map((x) => (
                                <TableRow
                                    key={x.adapterName}
                                    sx={{'&:last-child td, &:last-child th': {border: 0}}}
                                >
                                    <TableCell component="th" scope="row">
                                        {x.adapterName}
                                    </TableCell>
                                    <TableCell align="right">
                                        {Math.round(x.bandwidth / 1000000)} Mb/s
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </div>
            <div>
                <Stack direction={"row"} justifyContent={"space-between"} alignContent={"center"} flexWrap={"wrap"} gap={"10px"} marginBottom={"10px"}>
                    <div style={{display: 'flex', flexDirection: 'row', alignItems: 'center', gap: "10px"}}>
                        Sent/received
                        <FormControlLabel control={<Switch checked={showPercentage}
                                                           onChange={(event) => setShowPercentage(event.target.checked)}/>}
                                          label="Percentage"/>

                    </div>
                    <Button variant="contained" endIcon={<InsertChartIcon/>} size="small"
                            onClick={handleOpenChartClick} sx={{minWidth: '2em'}}>
                        All readings
                    </Button>
                </Stack>
                <CanvasJSChart options={GetOptions()} className={"usage-chart"}/>
            </div>
        </Paper>
    );
}