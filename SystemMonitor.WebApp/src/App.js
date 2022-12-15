import {Button, createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import SystemWidgetGroup from "./Components/SystemWidgetGroup";
import {CookiesProvider} from 'react-cookie';
import UsageChart from "./Components/Graph/UsageChart";
import {createBrowserRouter, Link, RouterProvider,} from "react-router-dom";
import {useState} from "react";

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

const router = createBrowserRouter([
    {
        path: "/",
        element: (
            <div>
                <SystemWidgetGroup/>
            </div>
        ),
    },
    {
        path: "chart/:id",
        element: (
            <div>
                <UsageChart/>
            </div>
        ),
    },
]);

function App() {
    const [currentTheme, setCurrentTheme] = useState(darkTheme)

    const handleChangeTheme = () => {
        if(currentTheme.palette.mode === 'dark') {
            setCurrentTheme(lightTheme)
        } else {
            setCurrentTheme(darkTheme)
        }
    }

    return (
        <ThemeProvider theme={currentTheme}>
            <CssBaseline/>
            <CookiesProvider>
                <Button onClick={handleChangeTheme}>
                    CHANGE THEME
                </Button>
                <RouterProvider router={router}/>
            </CookiesProvider>
        </ThemeProvider>
    );
}

export default App;
