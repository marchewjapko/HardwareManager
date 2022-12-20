import {useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import moment from "moment";
import {Paper, Slider} from "@mui/material";
import CpuDetails from "./CpuDetails";
import MemoryDetails from "./MemoryDetails";
import SystemDetailsCard from "./SystemDetailsCard";
import GetChartData from "./GetChartData";
import {useTheme} from "@mui/material/styles";
import DiskDetails from "./DiskDetails";
import CpuDetailsSkeleton from "./Skeletons/CpuDetailsSkeleton";
import MemoryDetailsSkeleton from "./Skeletons/MemoryDetailsSkeleton";
import DiskDetailsSkeleton from "./Skeletons/DiskDetailsSkeleton";

export default function SystemDetails({connection}) {
    const [system, setSystem] = useState(null);
    const [readings, setReadings] = useState([])
    const [isNotFound, setIsNotFound] = useState(false)
    const [readingMaxAgeMinutes, setReadingMaxAgeMinutes] = useState(JSON.parse(localStorage.getItem('reading-max-age')) || 5)
    const [sliderValue, setSliderValue] = useState(readingMaxAgeMinutes)
    const [lastTimestamp, setLastTimestamp] = useState(moment().subtract(readingMaxAgeMinutes, 'minutes').format())
    const navigate = useNavigate()
    const {id} = useParams();
    const theme = useTheme();

    useEffect(() => {
        setReadings([])
        setLastTimestamp(null)
        if (connection && connection.state !== 'Disconnected') {
            connection.on('ReceiveSystem', response => {
                if (response === null) {
                    setIsNotFound(true)
                } else {
                    setSystem(response)
                }
            });

            connection.send("GetSystem", parseInt(id), 1)
            connection.send("GetReadings", lastTimestamp, null, parseInt(id))
            return (() => {
                connection.off("ReceiveSystem")
            })
        }
    }, []);

    useEffect(() => {
        if (readings.length !== 0) {
            setLastTimestamp(moment().subtract(readingMaxAgeMinutes, 'minutes').format())
            setReadings([])
            connection.send("GetReadings", moment().subtract(readingMaxAgeMinutes, 'minutes').format(), null, parseInt(id))
        }
        connection.on('ReceiveReadings', response => {
            if (response === null) {
                setIsNotFound(true)
            } else if (response.length !== 0) {
                setReadings(readings => [...readings.filter((x) => moment.duration(moment().diff(x.timestamp)).asMinutes() < readingMaxAgeMinutes), ...response])
                setLastTimestamp(response[response.length - 1].timestamp)
            }
        })
        return (() => {
            connection.off("ReceiveReadings")
        })
    }, [readingMaxAgeMinutes]);

    useEffect(() => {
        if (readings.length !== 0) {
            setReadings([])
            setLastTimestamp(moment().subtract(readingMaxAgeMinutes, 'minutes').format())
            connection.send("GetReadings", moment().subtract(readingMaxAgeMinutes, 'minutes').format(), null, parseInt(id))
            connection.send("GetSystem", parseInt(id), 1)
        }
    }, [id]);


    useEffect(() => {
        const interval = setInterval(() => {
            connection.send("GetReadings", lastTimestamp, null, parseInt(id))
        }, 2000)
        return (() => {
            clearInterval(interval)
        })
    }, [id, lastTimestamp]);

    const handleDeleteSystem = (system) => {
        connection.send("DeleteSystem", system.id)
        navigate("/")
    }

    const handleChangeAuthorisation = (system) => {
        const newSystem = {
            id: system.id,
            isAuthorised: !system.isAuthorised,
            systemMacs: system.systemMacs,
            systemName: system.systemName
        }
        connection.send("UpdateSystem", newSystem, system.id)
        connection.send("GetSystem", parseInt(id), 1)
    }

    const handleChangeReadingMaxAge = (event, value) => {
        setReadingMaxAgeMinutes(value)
        if (JSON.parse(localStorage.getItem('reading-max-age'))) {
            localStorage.setItem('reading-max-age', value)
        } else {
            localStorage.setItem('reading-max-age', value)
        }
    }

    if (isNotFound) {
        return (
            <Paper className={"usage-chart-container"}>
                <div className={"usage-chart-not-found"}>
                    404 System not found &#128533;
                </div>
            </Paper>
        );
    }

    if (readings.length !== 0 && system) {
        return (
            <div className={"system-details-container"}>
                <div className={"system-details-primary-card-group"}>
                    <SystemDetailsCard system={system} handleDeleteSystem={handleDeleteSystem}
                                       handleChangeAuthorisation={handleChangeAuthorisation}/>
                    <Paper elevation={3} className={"system-details-slider"}>
                        <div>
                            Showing readings within last {readingMaxAgeMinutes} minute(s)
                        </div>
                        <Slider
                            value={sliderValue}
                            onChange={(e) => setSliderValue(e.target.value)}
                            valueLabelDisplay="auto"
                            step={1}
                            marks
                            min={1}
                            max={5}
                            onChangeCommitted={handleChangeReadingMaxAge}
                        />
                    </Paper>
                </div>
                <CpuDetails specs={readings[readings.length - 1].systemSpecsDTO}
                            dataPoints={GetChartData(readings, 'cpu', theme.palette.primary.main)}/>
                <MemoryDetails specs={readings[readings.length - 1].systemSpecsDTO}
                               dataPoints={GetChartData(readings, 'memory', theme.palette.primary.main)}
                               usedMemory={readings[readings.length - 1].usageDTO.memoryUsage}/>
                <DiskDetails specs={readings[readings.length - 1].systemSpecsDTO.diskSpecs}
                             dataPoints={GetChartData(readings, 'disks', theme.palette.primary.main)}/>
            </div>
        );
    }

    return (
        <div className={"system-details-container"}>
            {system && <div className={"system-details-primary-card-group"}>
                <Paper square={false} elevation={20} className={"system-details-main-card"}/>
                <Paper elevation={3} className={"system-details-slider"}>
                    <div>
                        Showing readings within last {readingMaxAgeMinutes} minute(s)
                    </div>
                    <Slider
                        value={sliderValue}
                        onChange={(e) => setSliderValue(e.target.value)}
                        valueLabelDisplay="auto"
                        step={1}
                        marks
                        min={1}
                        max={5}
                        onChangeCommitted={handleChangeReadingMaxAge}
                    />
                </Paper>
            </div>}
            {(readings.length !== 0 && system) &&
            <div>
                <CpuDetailsSkeleton/>
                <MemoryDetailsSkeleton/>
                <DiskDetailsSkeleton/>
            </div>}

        </div>
    );
}