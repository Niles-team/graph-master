import * as React from "react";
import { withStyles } from "@material-ui/core";
import { mergeStyles } from "../utils";
import { IsUnderConstruction } from "./common";

const styles = mergeStyles();

class DashboardBase extends React.Component {
    render() {
        return (
            <IsUnderConstruction/>
        );
    }
}

export const Dashboard = withStyles(styles)(DashboardBase);