import * as React from "react";
import { withStyles, WithStyles } from "@material-ui/styles";
import mergeStyles from "~/utils/mergeStyles";
import { Grid, Typography } from "@material-ui/core";

const styles = mergeStyles();

class DashboardBase extends React.Component<WithStyles<typeof styles>> {

    render() {
        return (
            <Grid direction="column">
                <Typography variant="h4">Graphs</Typography>
            </Grid>
        );
    }
}

export const Dashboard = withStyles(styles)(DashboardBase);