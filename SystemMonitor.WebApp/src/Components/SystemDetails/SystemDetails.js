import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import moment from "moment";
import {Paper} from "@mui/material";
import CpuDetails from "./CpuDetails";
import GetDataPoints from "./GetDataPoints";
import MemoryDetails from "./MemoryDetails";

export default function SystemDetails({connection}) {
    const [system, setSystem] = useState(null);
    const [readings, setReadings] = useState([])
    const [isNotFound, setIsNotFound] = useState(false)
    const {id} = useParams();

    useEffect(() => {
        if (connection && connection.state !== 'Disconnected') {
            connection.on('ReceiveSystem', response => {
                if (response === null) {
                    setIsNotFound(true)
                } else {
                    setSystem(response)
                }
            });
            connection.on('ReceiveReadings', response => {
                if (response === null) {
                    setIsNotFound(true)
                } else if (response.length !== 0) {
                    setReadings(response)
                }
            });
            connection.send("GetSystem", parseInt(id), 0)
            connection.send("GetReadings", moment().subtract(5, 'minutes').format(), null, parseInt(id))
            const interval = setInterval(() => {
                connection.send("GetReadings", moment().subtract(5, 'minutes').format(), null, parseInt(id))
            }, 3000)
            return (() => {
                clearInterval(interval)
            })
        }
    }, [id]);

    if (isNotFound) {
        return (
            <Paper className={"usage-chart-container"}>
                <div className={"usage-chart-not-found"}>
                    404 System not found &#128533;
                </div>
            </Paper>
        );
    }

    if (readings.length !== 0) {
        return (
            <div className={"system-details-container"}>
                <CpuDetails specs={readings[readings.length - 1].systemSpecsDTO}
                            dataPoints={GetDataPoints(readings, 'cpu-total')}/>
                <MemoryDetails specs={readings[readings.length - 1].systemSpecsDTO}
                               dataPoints={GetDataPoints(readings, 'memory')}
                               usedMemory={readings[readings.length - 1].usageDTO.memoryUsage}/>
            </div>
        );
    }

    return (
        <Paper className={"usage-chart-container"}>
            Loading...
        </Paper>
    );
}