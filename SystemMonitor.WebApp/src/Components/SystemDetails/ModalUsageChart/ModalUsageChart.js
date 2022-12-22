import React, {useEffect, useState} from 'react';
import {CanvasJSChart} from 'canvasjs-react-charts'
import {useTheme} from '@mui/material/styles'
import {Paper,} from "@mui/material";
import "./ModalUsageChart.js.css"
import SkeletonChart from "./SkeletonChart";
import moment from 'moment';
import GetChartData from "../GetChartData";
import {HubConnectionBuilder} from "@microsoft/signalr";

export default function ModalUsageChart({id, metric}) {
    const [readings, setReadings] = useState([]);
    const [connection, setConnection] = useState()
    const theme = useTheme();

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl("http://192.168.1.2:8080/systemInfoHub")
            .withAutomaticReconnect()
            .build();
        setConnection(newConnection);
        return (() => newConnection.stop())
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start().then(() => {
                connection.on('ReceiveGroupedReadings', response => {
                    setReadings(response)
                });
                connection.send("GetGroupedReadings", null, null, parseInt(id))
                return (() => {
                    connection.off('ReceiveGroupedReadings')
                })
            })
        }
    }, [connection]);

    function FilterDataPoints() {
        console.log("ALL CHART DATA", GetChartData(readings, 'disks', theme.palette.primary.main))
        switch (metric) {
            case 'cpu':
                return GetChartData(readings, 'cpu', theme.palette.primary.main).filter((x) => x.name === 'total')
            case 'memory':
                return GetChartData(readings, 'memory', theme.palette.primary.main)
            case 'disks':
                return GetChartData(readings, 'disks', theme.palette.primary.main)
            default:
                return GetChartData(readings, 'network', theme.palette.primary.main).filter((x) => x.name.includes('sent') || x.name.includes('received'))
        }
    }

    function GetChartName () {
        switch (metric) {
            case 'cpu':
                return "Total CPU usage"
            case 'memory':
                return "Total memory usage"
            case 'disks':
                return "Total disk(s) usage"
            default:
                return "Total network usage"
        }
    }

    function GetYAxisFormat() {
        switch (metric) {
            case 'cpu':
                return "##.##'%'"
            case 'memory':
                return "##.##'%'"
            case 'disks':
                return "##.##'%'"
            default:
                return "##.##kB/s"
        }
    }

    function GetOptions() {
        if (readings.length !== 0) {
            return {
                zoomEnabled: true,
                backgroundColor: "rgba(0,0,0,0)",
                fontSize: 50,
                theme: theme.palette.mode === 'light' ? "light2" : "dark1",
                animationEnabled: true,
                title: {
                    text: GetChartName(),
                    fontFamily: "Helvetica",
                    fontSize: 20,
                },
                axisX: {
                    valueFormatString: "DD-MM HH:mm"
                },
                axisY: {
                    valueFormatString: GetYAxisFormat(),
                    minimum: 0,
                },
                toolTip: {
                    contentFormatter: function (e) {
                        const date = moment(e.entries[0].dataPoint.x).format("DD.MM HH:mm")
                        const usage = Math.round(e.entries[0].dataPoint.y * 10) / 10
                        if(metric === 'network') {
                            return date + ' - ' + usage + ' KB/s';
                        }
                        return date + ' - ' + usage + '%';
                    }
                },
                data: FilterDataPoints()
            }
        }
    }

    if (readings.length !== 0) {
        return (
            <Paper className={"usage-chart-container"}>
                <CanvasJSChart options={GetOptions()} className={"usage-chart"}/>
            </Paper>
        );
    }

    return (
        <Paper className={"usage-chart-container"}>
            <SkeletonChart/>
        </Paper>
    );
}