import React, {useEffect, useState} from 'react';
import {CanvasJSChart} from 'canvasjs-react-charts'
import {useTheme} from '@mui/material/styles'
import moment from "moment";
import {Button, Checkbox, FormControl, FormControlLabel, FormLabel, Paper, Radio, RadioGroup} from "@mui/material";
import "./UsageChart.js.css"
import {useNavigate, useParams} from "react-router-dom";
import SkeletonChart from "./SkeletonChart";
import KeyboardReturnIcon from '@mui/icons-material/KeyboardReturn';
import {HubConnectionBuilder} from "@microsoft/signalr";

export default function UsageChart() {
    const [system, setSystem] = useState([]);
    const [connection, setConnection] = useState(null);
    const [isNotFound, setIsNotFound] = useState(false)
    const {id} = useParams();
    const theme = useTheme();
    const navigate = useNavigate();

    function Connect() {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://192.168.1.2:8080/systemInfoHub')
            .withAutomaticReconnect()
            .build();
        setConnection(newConnection);
    }

    useEffect(() => {
        Connect()
    }, []);

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
                    connection.send("GetSystem", parseInt(id), null)
                    const interval = setInterval(() => {
                        if(!connection) {
                            Connect()
                        }
                        if(breakInterval) {
                            clearInterval(interval);
                        }
                        connection.send("GetSystem", parseInt(id), null)
                    }, 5000)
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    function PrepareData() {
        let data = []
        data.push({
            x: new Date(system.systemReadingDTOs[0].timestamp),
            y: system.systemReadingDTOs[0].usageDTO.cpuTotalUsage
        })
        for (let i = 1; i < system.systemReadingDTOs.length; i++) {
            let timestamp1 = moment(system.systemReadingDTOs[i - 1].timestamp).utc()
            let timestamp2 = moment(system.systemReadingDTOs[i].timestamp).utc()
            const difference = timestamp1.diff(timestamp2)
            if (moment.duration(difference).asMinutes() > 1) {
                data.push({
                    x: new Date(timestamp2.add(parseInt(moment.duration(difference).asSeconds()), 's').toISOString()),
                    y: null
                })
            } else {
                data.push({
                    x: new Date(system.systemReadingDTOs[i].timestamp),
                    y: system.systemReadingDTOs[i].usageDTO.cpuTotalUsage
                })
            }
        }
        return data
    }

    function GetOptions() {
        if (system && system.systemReadingDTOs) {
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
                    connectNullData: true,
                    dataPoints: PrepareData()
                }]
            }
        }
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

    if (system && system.systemReadingDTOs) {
        return (
            <Paper className={"usage-chart-container"}>
                <div className={"usage-chart-header"}>
                    <div className={"usage-chart-system-title"}>
                        {system.systemName}
                    </div>

                    <FormControl>
                        <FormLabel filled>Chart options</FormLabel>
                        <FormControlLabel control={<Checkbox defaultChecked />} label="Only recent" />
                        <RadioGroup
                            defaultValue="cpu-total"
                        >
                            <FormControlLabel value="cpu-total" control={<Radio />} label="Total CPU" />
                            <FormControlLabel value="memory" control={<Radio />} label="Memory" />
                            <FormControlLabel value="other" control={<Radio />} label="Other" />
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