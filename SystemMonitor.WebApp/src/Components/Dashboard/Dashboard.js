import {useEffect, useState} from "react";
import {Alert, Backdrop, CircularProgress, Snackbar} from "@mui/material";
import SystemInfo from "./SystemInfoCard/SystemInfo";

export default function Dashboard({connection}) {
    const [isLoading, setIsLoading] = useState(true)
    const [systems, setSystems] = useState([])
    const [showSuccessAlert, setShowSuccessAlert] = useState(false)

    useEffect(() => {
        if (connection && connection.state !== 'Disconnected') {
            connection.on('ReceiveAllSystems', systems => {
                setSystems(systems)
                setIsLoading(false)
            });
            connection.send("BrowseAllSystems", 1)
            setInterval(() => {
                connection.send("BrowseAllSystems", 1)
            }, 2000)
        }
    }, []);

    const handleChangeAuthorisation = (system) => {
        const newSystem = {
            id: system.id,
            isAuthorised: !system.isAuthorised,
            systemMacs: system.systemMacs,
            systemName: system.systemName
        }
        connection.send("UpdateSystem", newSystem, system.id)
    }

    const handleDeleteSystem = (system) => {
        connection.send("DeleteSystem", system.id)
    }

    if (isLoading) {
        return (
            <div>
                <Backdrop
                    sx={{color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1}}
                    open={isLoading}
                >
                    <CircularProgress color="inherit"/>
                </Backdrop>
            </div>
        );
    }
    return (
        <div className={"system-info-widgets-container"}>
            <Snackbar open={showSuccessAlert} autoHideDuration={6000} onClose={() => setShowSuccessAlert(false)}>
                <Alert onClose={() => setShowSuccessAlert(false)} severity={"success"} sx={{width: '100%'}}>
                    System deleted
                </Alert>
            </Snackbar>
            {systems.map((x) => (
                <div key={x.id}>
                    <SystemInfo systemInfo={x} handleChangeAuthorisation={handleChangeAuthorisation}
                                handleDeleteSystem={handleDeleteSystem}/>
                </div>
            ))}
        </div>
    );
}