import * as React from "react";
import clsx from "clsx";

import { withStyles, Paper, Toolbar, Typography, Tooltip, IconButton, WithStyles } from "@material-ui/core";
import { Delete, FilterList } from "@material-ui/icons";

import { mergeStyles } from "../utils/mergeStyles";

const styles = mergeStyles();

interface State {
    selected: number;
}

class GraphsBase extends React.Component<WithStyles<typeof styles>, State> {
    constructor(props) {
        super(props);

        this.state = {
            selected: 0
        }
    }

    render() {
        const {classes} = this.props;
        const {selected} = this.state;

        return (
            <Paper>
                <Toolbar
                    className={clsx(classes.root, {
                        [classes.highlight]: selected > 0,
                    })}
                >
                    <div className={classes.title}>
                        {selected > 0 ? (
                            <Typography color="inherit" variant="subtitle1">
                                {selected} selected
                            </Typography>
                        ) : (
                                <Typography variant="h6" id="tableTitle">
                                    Graphs
                            </Typography>
                            )}
                    </div>
                    <div className={classes.spacer} />
                    <div className={classes.actions}>
                        {selected > 0 ? (
                            <Tooltip title="Delete">
                                <IconButton aria-label="delete">
                                    <Delete />
                                </IconButton>
                            </Tooltip>
                        ) : (
                                <Tooltip title="Filter list">
                                    <IconButton aria-label="filter list">
                                        <FilterList />
                                    </IconButton>
                                </Tooltip>
                            )}
                    </div>
                </Toolbar>
            </Paper>
        );
    }
}

export const Graphs = withStyles(styles)(GraphsBase);