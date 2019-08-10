import * as React from "react";

import { withStyles } from "@material-ui/core";
import { mergeStyles } from "../utils/mergeStyles";
import { IsUnderConstruction } from "./common";

const styles = mergeStyles();

class GraphicsBase extends React.Component {
    render() {
        return (
            <IsUnderConstruction/>
        );
    }
}

export const Graphics = withStyles(styles)(GraphicsBase);