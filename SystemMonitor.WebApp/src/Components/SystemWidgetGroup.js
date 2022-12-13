import {useEffect, useState} from "react";
import {Backdrop, CircularProgress} from "@mui/material";
import SystemInfo from "./SystemInfo/SystemInfo";
export default function SystemWidgetGroup() {
    const [isLoading, setIsLoading] = useState(true)
    const [systems, setSystems] = useState([])
    const [error, setError] = useState()
    useEffect(() => {
        fetch("http://192.168.1.2:8080/GetAllSystems?limit=0")
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
    }, [])

    if(error) {
        return (
            <div>
                ERROR: {error}
            </div>
        );
    }
    if(isLoading) {
        return (
            <div>
                <Backdrop
                    sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1 }}
                    open={isLoading}
                >
                    <CircularProgress color="inherit" />
                </Backdrop>
            </div>
        );
    }
    return(
        <div>
            {systems.map((x) => (
                <div key={x.id}>
                    <SystemInfo systemInfo={x}/>
                </div>
            ))}
        </div>
    );
}