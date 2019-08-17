import * as React from "react";
import clsx from "clsx";

import { Toolbar, Typography, Tooltip, IconButton } from "@material-ui/core";
import { WithStyles, withStyles } from "@material-ui/styles";

import { tableStyles } from "../../mui-theme";
import { mergeStyles } from "../../utils";
import { FilterList, Delete } from "@material-ui/icons";

const styles = mergeStyles(tableStyles);

interface Props extends WithStyles<typeof styles> {
    text: string;
    selected: number;
}

class TableToolbarBase extends React.Component<Props> {
    render() {
        const { text, selected, classes } = this.props;

        return (
            <Toolbar
              className={clsx(classes.toolBarRoot, {
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
                    {text}
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
            );
    }
}

export const TableToolbar = withStyles(styles)(TableToolbarBase);