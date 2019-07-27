import * as React from "react";
import { withStyles } from "@material-ui/core";
import mergeStyles from "../utils/mergeStyles";

const styles = mergeStyles();

class DashboardBase extends React.Component {
    render() {
        return (
            <h1>Hello world</h1>
        );
    }
}

export const Dashboard = withStyles(styles)(DashboardBase);