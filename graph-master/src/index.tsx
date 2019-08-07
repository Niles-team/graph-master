import * as React from "react";
import * as ReactDOM from "react-dom";
import { Router } from "react-router";

import { CssBaseline, MuiThemeProvider, createMuiTheme } from "@material-ui/core";

import { createBrowserHistory } from 'history';

import { routes } from "./routes";
import { sessionService } from "./services";

const logoColors = {
    blueHex: "#2EA5D8",
    greenHex: "#53C1AB",
    purpleHex: "#6F92CB",
};

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

sessionService.init();

const history = createBrowserHistory({ basename: baseUrl });

ReactDOM.render(
    <React.Fragment>
        <CssBaseline />
        <MuiThemeProvider theme={theme}>
            <Router history={history}>
                {routes}
            </Router>
        </MuiThemeProvider>
    </React.Fragment>,
    document.getElementById("app-container")
);