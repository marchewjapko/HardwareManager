import {useEffect, useState} from "react";
import {Alert, Backdrop, CircularProgress, Snackbar} from "@mui/material";
import SystemInfo from "./SystemInfo/SystemInfo";

export default function SystemWidgetGroup() {
    const [isLoading, setIsLoading] = useState(true)
    const [systems, setSystems] = useState([])
    const [error, setError] = useState(false)
    const [showSuccessAlert, setShowSuccessAlert] = useState(false)

    function GetAllSystems() {
        fetch("http://192.168.1.2:8080/GetAllSystems?limit=1")
            .then(res => res.json())
            .then(
                (result) => {
                    setIsLoading(false);
                    setSystems(result);
                },
                (error) => {
                    setIsLoading(false);
                    setError(error);
                }
            )
    }

    useEffect(() => {
        GetAllSystems()
        const interval = setInterval(() => {
            GetAllSystems()
        }, 2500);
        return () => clearInterval(interval);
    }, []);

    const handleChangeAuthorisation = async (system) => {
        const requestOptions = {
            method: 'PUT', headers: {'Content-Type': 'application/json'}, body: JSON.stringify({
                id: system.id,
                isAuthorised: !system.isAuthorised,
                systemMacs: system.systemMacs,
                systemName: system.systemName
            })
        };
        const response = await fetch('http://192.168.1.2:8080/UpdateSystem?id=' + system.id, requestOptions);
        if (response.ok) {
            GetAllSystems()
        }
    }

    const handleDeleteSystem = (system) => {
        let url = "http://192.168.1.2:8080/DeleteSystem?"
        system.systemMacs.forEach(x => url += "ids=" + x.replaceAll(':', '%3A') + '&')
        url = url.slice(0, -1)
        fetch(url, { method: 'DELETE' })
            .then((result) => {
                if(result.ok) {
                    setShowSuccessAlert(true)
                } else {
                    setError(true)
                }
            })
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
                <Alert onClose={() => setShowSuccessAlert(false)} severity={"success"} sx={{ width: '100%' }}>
                    System deleted
                </Alert>
            </Snackbar>
            <Snackbar open={error} autoHideDuration={6000} onClose={() => setError(false)}>
                <Alert onClose={() => setError(null)} severity={"error"} sx={{width: '100%'}}>
                    <div>
                        Something went wrong
                    </div>
                </Alert>
            </Snackbar>
            {systems.map((x) => (
                <div key={x.id}>
                    <SystemInfo systemInfo={x} handleChangeAuthorisation={handleChangeAuthorisation} handleDeleteSystem={handleDeleteSystem}/>
                </div>
            ))}
        </div>
    );
}