import * as React from "react";
import { RouteComponentProps } from "react-router";

import { withStyles, WithStyles, Grid, Typography } from "@material-ui/core";

import { mergeStyles } from "../utils";

const styles = mergeStyles();

interface Props extends WithStyles<typeof styles>, RouteComponentProps {
}

interface State {
    message?: string;
}

class ErrorBase extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);

        const { location: { state: locationState } } = this.props;

        this.state = {
            message: locationState && locationState.message
        }
    }

    render() {
        const { message } = this.state;

        return (
            <Grid container direction="column" component="main" alignItems="center" justify="center">
                <Typography variant="h1" component="h1">Oh no... Something went wrong...</Typography>
                <Typography variant="h3" component="h3">{message}</Typography>
            </Grid>
        );
    }
}

export const Error = withStyles(styles)(ErrorBase);