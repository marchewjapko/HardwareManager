import {createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import SystemWidgetGroup from "./Components/SystemWidgetGroup";
import {CookiesProvider} from 'react-cookie';
import UsageChart from "./Components/Graph/UsageChart";
import {createBrowserRouter, Link, RouterProvider,} from "react-router-dom";

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
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
    return (
        <ThemeProvider theme={darkTheme}>
            <CssBaseline/>
            <CookiesProvider>
                <RouterProvider router={router}/>
                {/*<SystemWidgetGroup/>*/}
                {/*<UsageChart/>*/}
            </CookiesProvider>
        </ThemeProvider>
    );
}

export default App;
