import * as React from "react";
import * as ReactDOM from "react-dom";
import { Router } from "react-router-dom";

import { CssBaseline } from "@material-ui/core";
import { MuiThemeProvider, createMuiTheme } from "@material-ui/core/styles";

import createHistory from 'history/createBrowserHistory';

import { App } from "./components/App";

const logoColors = {
    blueHex: "#2EA5D8",
    greenHex: "#53C1AB",
    purpleHex: "#6F92CB",
};

// createMuiTheme generates light, dark, and contrastText (when omitted) from of the main color
const theme = createMuiTheme({
    palette: {
        primary: {
            main: logoColors.blueHex,
            contrastText: "#fff",
        },
        secondary: {
            main: logoColors.purpleHex,
        },
    },
});

const baseUrl = document
    .getElementsByTagName("base")[0]
    .getAttribute("href")!;

const history = createHistory({ basename: baseUrl });

ReactDOM.render(
    <React.Fragment>
        <CssBaseline />
        <MuiThemeProvider theme={theme}>
            <Router history={history}>
                <App />
            </Router>
        </MuiThemeProvider>
    </React.Fragment>,
    document.getElementById("app-container")
);