import * as React from "react";
import { mergeStyles } from "../../utils";
import { layoutStyles } from "../../mui-theme";
import { withStyles, WithStyles } from "@material-ui/styles";
import { Grid, Typography } from "@material-ui/core";

const styles = mergeStyles(layoutStyles);

class IsUnderConstructionBase extends React.Component<WithStyles<typeof styles>> {
    render() {
        const { classes } = this.props;
        return (
            <Grid container direction="row" alignItems="center" justify="center" className={classes.root}>
                <Grid item xs />
                <Grid item>
                    <Typography variant="h5" component="h5">Sorry... Page is under construction</Typography>
                </Grid>
                <Grid item xs />
            </Grid>
        );
    }
}

export const IsUnderConstruction = withStyles(styles)(IsUnderConstructionBase);