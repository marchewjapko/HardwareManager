import {createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import SystemWidgetGroup from "./Components/SystemWidgetGroup";
import { CookiesProvider } from 'react-cookie';

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
    },
});


function App() {
  return (
      <ThemeProvider theme={darkTheme}>
          <CssBaseline />
          <CookiesProvider>
            <SystemWidgetGroup/>
          </CookiesProvider>
      </ThemeProvider>
  );
}

export default App;
