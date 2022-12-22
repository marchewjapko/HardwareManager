import {useEffect, useState} from "react";
import {Backdrop, CircularProgress} from "@mui/material";
import SystemInfoCard from "./SystemInfoCard/SystemInfoCard";

export default function Dashboard({connection}) {
    const [isLoading, setIsLoading] = useState(true)
    const [systems, setSystems] = useState([])

    useEffect(() => {
        connection.on('ReceiveAllSystems', response => {
            setSystems(response)
            if (isLoading) {
                setIsLoading(false)
            }
        });
        connection.send("BrowseAllSystems", 1)
        const interval = setInterval(() => {
            connection.send("BrowseAllSystems", 1)
        }, 2000)
        return (() => {
            connection.off("ReceiveAllSystems")
            clearInterval(interval)
        })
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
        setTimeout(() => {
                return (
                    <div>
                        <Backdrop
                            sx={{color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1}}
                            open={isLoading}
                        >
                            <CircularProgress color="inherit"/>
                        </Backdrop>
                    </div>
                )
            }, 5000
        )
    }
    return (
        <div className={"system-info-widgets-container"}>
            <div className={"system-info-card-group"}>
                {systems.map((x) => (
                    <div key={x.id}>
                        <SystemInfoCard system={x} handleChangeAuthorisation={handleChangeAuthorisation}
                                        handleDeleteSystem={handleDeleteSystem}/>
                    </div>

                ))}
            </div>
        </div>
    );
}