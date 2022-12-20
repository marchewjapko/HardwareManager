import {createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import Dashboard from "./Components/Dashboard/Dashboard";
import {CookiesProvider, useCookies} from 'react-cookie';
import UsageChart from "./Components/UsageChart/UsageChart";
import {createBrowserRouter, Outlet, RouterProvider,} from "react-router-dom";
import React, {useEffect, useState} from 'react';
import SystemDetails from "./Components/SystemDetails/SystemDetails";
import {HubConnectionBuilder} from "@microsoft/signalr";
import Header from "./Components/Header/Header";

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
    const [connection, setConnection] = useState()
    const [isLoading, setIsLoading] = useState(true)
    const handleChangeTheme = () => {
        if (isLightMode['lightMode'] === 'true') {
            setIsLightMode('lightMode', false, {path: '/', expires: new Date(2147483647 * 1000), sameSite: "strict"})
        } else {
            setIsLightMode('lightMode', true, {path: '/', expires: new Date(2147483647 * 1000), sameSite: "strict"})
        }
    }
    // useEffect(() => {
    //     const newConnection = new HubConnectionBuilder()
    //         .withUrl("https://localhost:7298/systemInfoHub")
    //         .withAutomaticReconnect()
    //         .build();
    //     setConnection(newConnection);
    // }, []);

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl("http://192.168.1.2:8080/systemInfoHub")
            .withAutomaticReconnect()
            .build();
        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start().then(() => setIsLoading(false))
        }
    }, [connection]);

    if (isLoading) {
        return (
            <ThemeProvider theme={isLightMode['lightMode'] === 'true' ? lightTheme : darkTheme}>
                <CssBaseline/>
                Loading
            </ThemeProvider>
        );
    } else {
        const AppLayout = () => (
            <>
                <Header handleChangeTheme={handleChangeTheme} connection={connection}/>
                <Outlet/>
            </>
        );

        function GetRouter() {
            return new createBrowserRouter([
                {
                    element: <AppLayout/>,
                    children: [
                        {
                            path: "/",
                            element: <Dashboard connection={connection}/>,
                        },
                        {
                            path: "system/:id",
                            element: <SystemDetails connection={connection}/>,
                        },
                        {
                            path: "chart/:id",
                            element: <UsageChart connection={connection}/>,
                        },
                    ],
                },
            ])
        }

        return (
            <ThemeProvider theme={isLightMode['lightMode'] === 'true' ? lightTheme : darkTheme}>
                <CssBaseline/>
                <CookiesProvider>
                    <RouterProvider router={GetRouter()}/>
                </CookiesProvider>
            </ThemeProvider>
        );
    }
}

export default App;
