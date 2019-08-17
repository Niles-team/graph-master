import * as React from "react";

import { TableHead, TableRow, TableCell, Checkbox, TableSortLabel } from "@material-ui/core";
import { withStyles, WithStyles } from "@material-ui/styles";

import { mergeStyles } from "../../utils";
import { tableStyles } from "../../mui-theme";
import { Column, Order } from "../../models";

const styles = mergeStyles(tableStyles);

interface Props extends WithStyles<typeof styles> {
    selected: number;
    rowCount: number;
    appendCheckBox: boolean;
    columns: Column[];
    order: Order;
    orderBy: string;

    onSelectAllClick: (event: React.ChangeEvent<HTMLInputElement>, checked: boolean) => void;
    onSortColumnClick: (event: React.MouseEvent<unknown>, property: string) => void;
}

class TableHeadBase extends React.Component<Props> {
    private createSortColumnClickHandler = (property: string) => (event: React.MouseEvent<unknown>) => {
        const { onSortColumnClick } = this.props;
        onSortColumnClick(event, property);
    }

    render() {
        const {
            classes,
            selected,
            rowCount,
            appendCheckBox,
            columns,
            order,
            orderBy,
            onSelectAllClick
        } = this.props;

        const renderedColumns = columns.map(column => (
            <TableCell
                key={column.id}
                align={column.align}
                padding={column.disablePadding ? 'none' : 'default'}
                sortDirection={orderBy === column.id ? order : false}
                variant="head"
            >
                {!column.disableSort ? (
                    <TableSortLabel
                        active={orderBy === column.id}
                        direction={order}
                        onClick={this.createSortColumnClickHandler(column.id)}
                    >
                        {column.label}
                        {orderBy === column.id ? (
                            <span className={classes.visuallyHidden}>
                                {order === Order.desc ? 'sorted descending' : 'sorted ascending'}
                            </span>
                        ) : null}
                    </TableSortLabel>
                ) : (
                        column.label
                    )}
            </TableCell>
        ));

        return (
            <TableHead>
                <TableRow>
                    {appendCheckBox &&
                        <TableCell padding="checkbox">
                            <Checkbox
                                indeterminate={selected > 0 && selected < rowCount}
                                checked={selected === rowCount}
                                onChange={onSelectAllClick}
                                inputProps={{ 'aria-label': 'select all desserts' }}
                            />
                        </TableCell>
                    }
                    {renderedColumns}
                </TableRow>
            </TableHead>
        );
    }
}

export const MaterialTableHead = withStyles(styles)(TableHeadBase);