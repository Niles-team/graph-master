import * as React from "react";

import { withStyles, Paper, WithStyles } from "@material-ui/core";

import { mergeStyles } from "../utils/mergeStyles";
import { ReactTable } from "./common";
import { Column } from "../models";

const styles = mergeStyles();

interface State {
}

class GraphsBase extends React.Component<WithStyles<typeof styles>, State> {
    constructor(props) {
        super(props);

        this.state = {
            selected: 0
        }
    }

    render() {
        const { classes } = this.props;

        const columns: Column[] = [{
            id: 'name',
            label: 'Name',
            align: 'justify',
            disableSort: false,
            disablePadding: true
        }, {
            id: 'date',
            label: 'Date',
            align: 'right',
            disableSort: true,
            disablePadding: false
        }];

        const data = [{
            name: 'new graph',
            date: new Date().toISOString(),
            selected: true
        }];

        return (
            <Paper>
                <ReactTable 
                    data={data} 
                    title={"Your Graphs"} 
                    appendCheckbox={true} 
                    columns={columns} 
                    dense={false}
                />
            </Paper>
        );
    }
}

export const Graphs = withStyles(styles)(GraphsBase);