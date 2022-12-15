import React, {useEffect, useState} from 'react';
import {CanvasJSChart} from 'canvasjs-react-charts'
import {useTheme} from '@mui/material/styles'
import moment from "moment";
import {Button, Paper} from "@mui/material";
import "./UsageChart.js.css"
import {useNavigate, useParams} from "react-router-dom";
import SkeletonChart from "./SkeletonChart";
import KeyboardReturnIcon from '@mui/icons-material/KeyboardReturn';

export default function UsageChart() {
    const [systemReadings, setSystemReadings] = useState([]);
    const [error, setError] = useState(false)
    const [isNotFound, setIsNotFound] = useState(false)
    const {id} = useParams();
    const theme = useTheme();
    const navigate = useNavigate();

    function GetReadings() {
        let url = "http://192.168.1.2:8080/GetSystemID?id=" + id
        fetch(url, {method: 'GET'})
            .then(res => res.json())
            .then(
                (result) => {
                    if (result.status === 404) {
                        setIsNotFound(true)
                    }
                    setSystemReadings(result.systemReadingDTOs)
                },
                (error) => {
                    setError(true)
                }
            )
    }

    useEffect(() => {
        if (!isNotFound) {
            GetReadings()
            const interval = setInterval(() => {
                GetReadings()
            }, 5000);
            return () => clearInterval(interval);
        }
    }, []);

    function PrepareData() {
        let data = []
        data.push({
            x: new Date(systemReadings[0].timestamp),
            y: systemReadings[0].usageDTO.cpuTotalUsage
        })
        for (let i = 1; i < systemReadings.length; i++) {
            let timestamp1 = moment(systemReadings[i - 1].timestamp).utc()
            let timestamp2 = moment(systemReadings[i].timestamp).utc()
            const difference = timestamp1.diff(timestamp2)
            if (moment.duration(difference).asMinutes() > 1) {
                data.push({
                    x: new Date(timestamp2.add(parseInt(moment.duration(difference).asSeconds()), 's').toISOString()),
                    y: null
                })
            } else {
                data.push({
                    x: new Date(systemReadings[i].timestamp),
                    y: systemReadings[i].usageDTO.cpuTotalUsage
                })
            }
        }
        return data
    }

    function GetOptions() {
        if (systemReadings[0]) {
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

    if(isNotFound) {
        return (
            <Paper className={"usage-chart-container"}>
                <Button variant="contained" endIcon={<KeyboardReturnIcon />} onClick={() => navigate('/')}>
                    Return
                </Button>
                <div>
                    404 System not found :/
                </div>
            </Paper>
        );
    }

    if(systemReadings && systemReadings[0]) {
        return (
            <Paper className={"usage-chart-container"}>
                <Button variant="contained" endIcon={<KeyboardReturnIcon />} onClick={() => navigate('/')}>
                    Return
                </Button>
                <CanvasJSChart options={GetOptions()} className={"usage-chart"}/>
            </Paper>
        );
    }

    return (
        <Paper className={"usage-chart-container"}>
            <Button variant="contained" endIcon={<KeyboardReturnIcon />} onClick={() => navigate('/')}>
                Return
            </Button>
            <SkeletonChart/>
        </Paper>
    );
}