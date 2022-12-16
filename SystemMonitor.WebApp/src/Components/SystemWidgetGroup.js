import {useEffect, useState} from "react";
import {Alert, Backdrop, CircularProgress, Snackbar} from "@mui/material";
import SystemInfo from "./SystemInfo/SystemInfo";
import {HubConnectionBuilder} from "@microsoft/signalr";

export default function SystemWidgetGroup() {
    const [connection, setConnection] = useState(null);
    const [isLoading, setIsLoading] = useState(true)
    const [systems, setSystems] = useState([])
    const [showSuccessAlert, setShowSuccessAlert] = useState(false)

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://192.168.1.2:8080/systemInfoHub')
            .withAutomaticReconnect()
            .build();
        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    connection.on('ReceiveAllSystems', systems => {
                        setSystems(systems)
                        setIsLoading(false)
                    });
                    connection.send("BrowseAllSystems", 1)
                    setInterval(() => {
                        connection.send("BrowseAllSystems", 1)
                    }, 2000)
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

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