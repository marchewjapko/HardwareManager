import {createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import Dashboard from "./Components/Dashboard/Dashboard";
import {CookiesProvider, useCookies} from 'react-cookie';
import UsageChart from "./Components/UsageChart/UsageChart";
import {createBrowserRouter, Outlet, RouterProvider,} from "react-router-dom";
import React from 'react';
import SystemDetails from "./Components/SystemDetails/SystemDetails";
import Header from "./Components/Header/Header";
import {useEffect, useState} from "react";
import {HubConnectionBuilder} from "@microsoft/signalr";

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
    },
});

const lightTheme = createTheme({
    palette: {
        mode: 'light',
    },
});

function App() {
    const [isLightMode, setIsLightMode] = useCookies(['lightMode']);
    const [connection, setConnection] = useState(null)
    const handleChangeTheme = () => {
        if (isLightMode['lightMode'] === 'true') {
            setIsLightMode('lightMode', false, {path: '/', sameSite: "lax"})
        } else {
            setIsLightMode('lightMode', true, {path: '/', sameSite: "lax"})
        }
    }

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://192.168.1.2:8080/systemInfoHub')
            .withAutomaticReconnect()
            .build();
        newConnection.start().then(() => setConnection(newConnection));
    }, []);

    const AppLayout = () => (
        <>
            <Header handleChangeTheme={handleChangeTheme} connection={connection}/>
            <Outlet />
        </>
    );

    function GetRouter() {
        return new createBrowserRouter([
            {
                element: <AppLayout />,
                children: [
                    {
                        path: "/",
                        element: <Dashboard connection={connection} />,
                    },
                    {
                        path: "system/:id",
                        element: <SystemDetails connection={connection} />,
                    },
                    {
                        path: "chart/:id",
                        element: <UsageChart />,
                    },
                ],
            },
        ])
    }
    return (
        <React.StrictMode>
        <ThemeProvider theme={isLightMode['lightMode'] === 'true' ? lightTheme : darkTheme}>
            <CssBaseline/>
            <CookiesProvider>
                {connection && connection.state !== 'Disconnected' ? (
                    <RouterProvider router={GetRouter()}/>
                ) : (
                    <div>
                        Loading
                    </div>
                )}
            </CookiesProvider>
        </ThemeProvider>
        </React.StrictMode>
    );
}

export default App;
