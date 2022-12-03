import SystemInfo from "./Components/SystemInfo/SystemInfo";
import {createTheme, CssBaseline, ThemeProvider} from "@mui/material";

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
    },
});


function App() {
  return (
      <ThemeProvider theme={darkTheme}>
          <CssBaseline />
          <SystemInfo/>
      </ThemeProvider>
  );
}

export default App;
