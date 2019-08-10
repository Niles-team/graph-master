import * as React from "react";

import { withStyles } from "@material-ui/core";
import { mergeStyles } from "../utils/mergeStyles";
import { IsUnderConstruction } from "./common";

const styles = mergeStyles();

class GraphsBase extends React.Component {
    render() {
        return (
            <IsUnderConstruction/>
        );
    }
}

export const Graphs = withStyles(styles)(GraphsBase);