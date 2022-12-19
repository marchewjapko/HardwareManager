import React, {useEffect, useRef, useState} from 'react';
import {CanvasJSChart} from 'canvasjs-react-charts'
import {useTheme} from '@mui/material/styles'
import {Button, Checkbox, FormControl, FormControlLabel, FormLabel, Paper, Radio, RadioGroup,} from "@mui/material";
import "./UsageChart.js.css"
import {useNavigate, useParams} from "react-router-dom";
import SkeletonChart from "./SkeletonChart";
import KeyboardReturnIcon from '@mui/icons-material/KeyboardReturn';
import {HubConnectionBuilder} from "@microsoft/signalr";
import GetGraphData from "./GetGraphData";
import moment from 'moment';

export default function UsageChart() {
    const [connection, setConnection] = useState(null);
    const [system, setSystem] = useState([]);
    const [dataPoints, setDataPoints] = useState([]);
    const [isNotFound, setIsNotFound] = useState(false)
    const [selectedMetric, setSelectedMetric] = useState('cpu-total')
    const [onlyRecent, setOnlyRecent] = useState(true)
    const theme = useTheme();
    const navigate = useNavigate();
    const {id} = useParams();

    const lastTimestamp = useRef(null);

    useEffect(() => {
        let breakInterval = false
        if (connection) {
            connection.start()
                .then(result => {
                    connection.on('ReceiveSystem', response => {
                        if (response === null) {
                            setIsNotFound(true)
                            breakInterval = true
                        } else {
                            setSystem(response)
                        }
                    });
                    connection.on('ReceiveReadings', response => {
                        if (response === null) {
                            setIsNotFound(true)
                            breakInterval = true
                        } else if (response.length !== 0) {
                            setDataPoints((dataPoints) => [...dataPoints, ...GetGraphData(response, dataPoints[dataPoints.length - 1])])
                            lastTimestamp.current = response[response.length - 1].timestamp
                        }
                    });
                    connection.send("GetSystem", parseInt(id), 0)
                    const GetFirstReadings = async () => {
                        await connection.send("GetReadings", null, null, parseInt(id))
                    }
                    GetFirstReadings().then(() => {
                        const interval = setInterval(() => {
                            if (breakInterval) {
                                clearInterval(interval);
                            } else if (lastTimestamp.current) {
                                connection.send("GetReadings", lastTimestamp.current, null, parseInt(id))
                            }
                        }, 5000)
                    })
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    function GetOptions() {
        if (dataPoints.length !== 0) {
            return {
                zoomEnabled: true,
                backgroundColor: "rgba(0,0,0,0)",
                fontSize: 50,
                theme: theme.palette.mode === 'light' ? "light2" : "dark1",
                animationEnabled: true,
                title: {
                    text: "Total CPU usage",
                    fontFamily: "Helvetica",
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
                    dataPoints: GetDataPoints()
                }]
            }
        }
    }

    function GetDataPoints() {
        let result = []
        if (selectedMetric === 'cpu-total') {
            result = dataPoints.map(v => ({x: v.x, y: v.y.totalCPU}))
        } else {
            result = dataPoints.map(v => ({x: v.x, y: v.y.memory}))
        }
        if (onlyRecent) {
            result = result.filter((x) => moment.duration(moment().diff(x.x)).asMinutes() < 20)
        }
        return result
    }

    if (isNotFound) {
        return (
            <Paper className={"usage-chart-container"}>
                <Button variant="contained" endIcon={<KeyboardReturnIcon/>} onClick={() => navigate('/')}>
                    Return
                </Button>
                <div className={"usage-chart-not-found"}>
                    404 System not found &#128533;
                </div>
            </Paper>
        );
    }

    if (dataPoints.length !== 0) {
        return (
            <Paper className={"usage-chart-container"}>
                <div className={"usage-chart-header"}>
                    <div className={"usage-chart-system-title"}>
                        {system.systemName}
                    </div>

                    <FormControl>
                        <FormLabel filled>Chart options</FormLabel>
                        <FormControlLabel control={<Checkbox checked={onlyRecent}
                                                             onChange={(event) => setOnlyRecent(event.target.checked)}/>}
                                          label="Only recent"/>
                        <RadioGroup
                            value={selectedMetric}
                            onChange={(event) => setSelectedMetric(event.target.value)}
                        >
                            <FormControlLabel value="cpu-total" control={<Radio/>} label="Total CPU"/>
                            <FormControlLabel value="memory" control={<Radio/>} label="Memory"/>
                            <FormControlLabel value="other" control={<Radio/>} label="Other"/>
                        </RadioGroup>
                    </FormControl>

                    <Button variant="contained" endIcon={<KeyboardReturnIcon/>} onClick={() => navigate('/')}>
                        Return
                    </Button>
                </div>
                <CanvasJSChart options={GetOptions()} className={"usage-chart"}/>
            </Paper>
        );
    }

    return (
        <Paper className={"usage-chart-container"}>
            <Button variant="contained" endIcon={<KeyboardReturnIcon/>} onClick={() => navigate('/')}>
                Return
            </Button>
            <SkeletonChart/>
        </Paper>
    );
}