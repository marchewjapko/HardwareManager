import {useNavigate, useParams} from "react-router-dom";
import {useEffect, useRef, useState} from "react";
import moment from "moment";
import {Dialog, DialogTitle, IconButton, Paper, Slider, Stack} from "@mui/material";
import CpuDetails from "./CpuDetails";
import MemoryDetails from "./MemoryDetails";
import SystemDetailsCard from "./SystemDetailsCard";
import GetChartData from "./GetChartData";
import {useTheme} from "@mui/material/styles";
import DiskDetails from "./DiskDetails";
import CpuDetailsSkeleton from "./Skeletons/CpuDetailsSkeleton";
import MemoryDetailsSkeleton from "./Skeletons/MemoryDetailsSkeleton";
import DiskDetailsSkeleton from "./Skeletons/DiskDetailsSkeleton";
import Masonry from '@mui/lab/Masonry';
import SystemDetailsCardSkeleton from "./Skeletons/SystemDetailsCardSkeleton";
import NetworkDetails from "./NetworkDetails";
import NetworkDetailsSkeleton from "./Skeletons/NetworkDetailsSkeleton";
import ModalUsageChart from "./ModalUsageChart/ModalUsageChart";
import CloseIcon from '@mui/icons-material/Close';

export default function SystemDetails({connection}) {
    const [system, setSystem] = useState(null);
    const [readings, setReadings] = useState([])
    const [isNotFound, setIsNotFound] = useState(false)
    const [readingMaxAgeMinutes, setReadingMaxAgeMinutes] = useState(JSON.parse(localStorage.getItem('reading-max-age')) || 5)
    const [sliderValue, setSliderValue] = useState(readingMaxAgeMinutes)
    const [lastTimestamp, setLastTimestamp] = useState(moment().subtract(readingMaxAgeMinutes, 'minutes').format())
    const [isDialogOpen, setIsDialogOpen] = useState(false)
    const [usageModalMetric, setUsageModalMetric] = useState()
    const navigate = useNavigate()
    const {id} = useParams();
    const theme = useTheme();

    const didMountRef = useRef(false);

    useEffect(() => {
        setReadings([])
        let oldSystem;
        if (connection && connection.state !== 'Disconnected') {
            connection.on('ReceiveSystem', response => {
                if (response === null) {
                    setIsNotFound(true)
                } else {
                    setSystem(response)
                    oldSystem = response
                }
            });

            connection.on("ReceiveAllSystems", response => {
                const newSystem = response.filter((x) => x.id === parseInt(id))[0]
                if (!newSystem) {
                    setIsNotFound(true)
                } else if (newSystem.isAuthorised !== oldSystem.isAuthorised) {
                    setSystem(newSystem)
                    oldSystem = newSystem
                }
            })

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
            console.log("RECEIVED READINGSA!", response)
            setReadings(readings => [...readings, ...response].filter((x) => moment.duration(moment().diff(x.timestamp)).asMinutes() < readingMaxAgeMinutes))
            setLastTimestamp(response[response.length - 1].timestamp)
        })
        return (() => {
            connection.off("ReceiveReadings")
        })
    }, [readingMaxAgeMinutes]);

    useEffect(() => {
        if (didMountRef.current) {
            connection.send("GetSystem", parseInt(id), 1)
            setReadings([])
            setLastTimestamp(moment().subtract(readingMaxAgeMinutes, 'minutes').format())
            connection.send("GetReadings", moment().subtract(readingMaxAgeMinutes, 'minutes').format(), null, parseInt(id))
        }
        didMountRef.current = true
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

    function GetModalTitle() {
        switch (usageModalMetric) {
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

    return (
        <div className={"system-details-container"}>
            <Masonry columns={{xs: 1, sm: 1, md: 2, lg: 4}} spacing={2}>
                <div className={"system-details-primary-card-group"}>
                    {system ? (
                        <SystemDetailsCard system={system} handleDeleteSystem={handleDeleteSystem}
                                           handleChangeAuthorisation={handleChangeAuthorisation}/>
                    ) : (
                        <SystemDetailsCardSkeleton/>
                    )}
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
                            disabled={!system}
                            onChangeCommitted={handleChangeReadingMaxAge}
                        />
                    </Paper>
                </div>
                {readings.length !== 0 &&
                    <CpuDetails specs={readings[readings.length - 1].systemSpecsDTO} setIsDialogOpen={setIsDialogOpen}
                                setUsageModalMetric={setUsageModalMetric}
                                dataPoints={GetChartData(readings, 'cpu', theme.palette.primary.main)}/>}
                {readings.length !== 0 &&
                    <MemoryDetails specs={readings[readings.length - 1].systemSpecsDTO}
                                   setIsDialogOpen={setIsDialogOpen} setUsageModalMetric={setUsageModalMetric}
                                   dataPoints={GetChartData(readings, 'memory', theme.palette.primary.main)}
                                   usedMemory={readings[readings.length - 1].usageDTO.memoryUsage}/>}
                {readings.length !== 0 &&
                    <DiskDetails specs={readings[readings.length - 1].systemSpecsDTO.diskSpecs}
                                 setIsDialogOpen={setIsDialogOpen} setUsageModalMetric={setUsageModalMetric}
                                 dataPoints={GetChartData(readings, 'disks', theme.palette.primary.main)}/>}
                {readings.length !== 0 &&
                    <NetworkDetails specs={readings[readings.length - 1].systemSpecsDTO.networkSpecs}
                                    setIsDialogOpen={setIsDialogOpen} setUsageModalMetric={setUsageModalMetric}
                                    dataPoints={GetChartData(readings, 'network', theme.palette.primary.main)}/>}

                {readings.length === 0 && <CpuDetailsSkeleton/>}
                {readings.length === 0 && <MemoryDetailsSkeleton/>}
                {readings.length === 0 && <DiskDetailsSkeleton/>}
                {readings.length === 0 && <NetworkDetailsSkeleton/>}
            </Masonry>
            <Dialog onClose={() => setIsDialogOpen(false)} open={isDialogOpen} fullWidth
                    className={"system-details-dialog"} maxWidth={"xl"}>
                <DialogTitle>
                    <Stack direction={"row"} justifyContent={"space-between"}>
                        {GetModalTitle()}
                        <IconButton aria-label="delete" size="small" onClick={() => setIsDialogOpen(false)}>
                            <CloseIcon/>
                        </IconButton>
                    </Stack>
                </DialogTitle>
                <ModalUsageChart id={id} metric={usageModalMetric}/>
            </Dialog>
        </div>
    );
}