import moment from "moment";
import {useTheme} from "@mui/material/styles";
import {CanvasJSChart} from "canvasjs-react-charts";
import "./SystemDetails.js.css"
import {
    Checkbox,
    FormControlLabel,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow
} from "@mui/material";
import {useState} from "react";
import {useEffect} from "react";

export default function CpuDetails({dataPoints, specs}) {
    const [splitByCores, setSplitByCores] = useState(JSON.parse(localStorage.getItem('splitByCores')) || false)
    const theme = useTheme();

    useEffect(() => {
        if (JSON.parse(localStorage.getItem('splitByCores')) === null) {
            localStorage.setItem('splitByCores', 'false')
        }
    }, []);

    const handleChange = (event) => {
        setSplitByCores(event.target.checked)
        if (JSON.parse(localStorage.getItem('splitByCores'))) {
            localStorage.setItem('splitByCores', 'false')
        } else {
            localStorage.setItem('splitByCores', 'true')
        }
    }

    function GetOptions() {
        if (dataPoints.length !== 0) {
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
                    shared: true,
                    contentFormatter: function (e) {
                        let content = ""
                        if (e.entries.length === 1) {
                            const date = moment(e.entries[0].dataPoint.x).format("DD.MM HH:mm:ss")
                            const usage = Math.round(e.entries[0].dataPoint.y * 10) / 10
                            return date + " - " + usage
                        }
                        return moment(e.entries[0].dataPoint.x).format("DD.MM HH:mm:ss")

                        // for (let i = 0; i < e.entries.length; i++) {
                        //     content += "Core#" + e.entries[i].dataSeries.name + " - " + "<strong>" + Math.round(e.entries[i].dataPoint.y * 10) / 10 + "%</strong>";
                        //     content += "<br/>";
                        // }
                        // content += "Time: " + moment(e.entries[0].dataPoint.x).format("DD.MM HH:mm:ss")
                        // return content;
                    }
                },
                data: splitByCores ? dataPoints.filter((x) => x.name !== 'total') : dataPoints.filter((x) => x.name === 'total')
            }
        }
    }

    return (
        <Paper className={"system-details-card-container"} elevation={3}>
            <div className={"system-details-card-title"}>
                CPU
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
                                    CPU
                                </TableCell>
                                <TableCell align="right">
                                    {specs.cpuInfo}
                                </TableCell>
                            </TableRow>
                            <TableRow
                                key={1}
                            >
                                <TableCell component="th" scope="row">
                                    Number of cores
                                </TableCell>
                                <TableCell align="right">
                                    {specs.cpuCores}
                                </TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </TableContainer>
            </div>
            <div>
                <FormControlLabel
                    control={<Checkbox checked={splitByCores} onChange={handleChange}/>}
                    label="Split by cores"/>
                <CanvasJSChart options={GetOptions()} className={"usage-chart"}/>
            </div>
        </Paper>
    );
}