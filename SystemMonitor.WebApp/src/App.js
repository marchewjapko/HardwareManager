import SystemInfo from "./Components/SystemInfo/SystemInfo";
import {createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import SystemWidgetGroup from "./Components/SystemWidgetGroup";

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
    },
});


function App() {
  return (
      <ThemeProvider theme={darkTheme}>
          <CssBaseline />
          <SystemWidgetGroup/>
      </ThemeProvider>
  );
}

export default App;
