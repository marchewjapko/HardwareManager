import {useCookies} from "react-cookie";
import {useEffect, useState} from "react";
import SystemControlCard from "../Shared/SystemControlCard/SystemControlCard";
import {Paper} from "@mui/material";

export default function SystemDetailsCard({system, handleDeleteSystem, handleChangeAuthorisation}) {
    const [cookie, setCookie] = useCookies(['systemAlias' + system.id]);

    useEffect(() => {
        if (!cookie['systemAlias' + system.id]) {
            setCookie('systemAlias' + system.id, system.systemName, {
                path: '/',
                expires: new Date(2147483647 * 1000),
                sameSite: "lax"
            })
        }
        if (!cookie['systemColor' + system.id]) {
            setCookie('systemColor' + system.id, "rgba(0, 0, 0, 0)", {
                path: '/',
                expires: new Date(2147483647 * 1000),
                sameSite: "lax"
            })
        }
    }, []);

    return (
        <Paper square={false} elevation={20}>
            <SystemControlCard system={system} handleChangeAuthorisation={handleChangeAuthorisation}
                               handleDeleteSystem={handleDeleteSystem}/>
        </Paper>
    );
}