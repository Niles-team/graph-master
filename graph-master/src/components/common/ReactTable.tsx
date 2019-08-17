import * as React from "react";

import { WithStyles, withStyles } from "@material-ui/styles";

import { mergeStyles } from "../../utils";
import { tableStyles } from "../../mui-theme";
import { Column, Order } from "../../models";
import { MaterialTableHead } from "./TableHead";
import { TableToolbar } from "./TableToolbar";
import { Table, TableBody, TableRow, Checkbox, TableCell, TablePagination } from "@material-ui/core";

const styles = mergeStyles(tableStyles);

interface Props extends WithStyles<typeof styles> {
    title: string;
    data: any[];
    columns: Column[];
    dense?: boolean;
    appendCheckbox?: boolean;
}

interface State {
    order: Order;
    orderBy?: string;
    selected: string[];
    page: number;
    rowsPerPage: number;
}

class ReactTableBase extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);

        this.state = {
            order: Order.asc,
            selected: [],
            page: 0,
            rowsPerPage: 5
        }
    }

    private handleSortColumnClick = (event: React.MouseEvent<unknown>, property: string) => {
        const { order, orderBy } = this.state;
        const isDesc = orderBy === property && order === Order.desc;
        this.setState({
            order: isDesc ? Order.asc : Order.desc,
            orderBy: property
        });
    }

    private handleSelectAllClick = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { data } = this.props;
        let selected = [];
        if (event.target && event.target.value) {
            selected = data.map(o => o.name);
        }

        this.setState({ selected });
    }

    private handleRowClick = (event: React.MouseEvent<unknown>, name: string) => {
        const { selected } = this.state;
        const selectedIndex = selected.indexOf(name);

        let newSelected: string[] = [];
        if (selectedIndex === 0) {
            newSelected = newSelected.concat(selected.slice(1));
        }
        else if (selectedIndex === selected.length - 1) {
            newSelected = newSelected.concat(selected.slice(0, -1));
        }
        else if (selectedIndex > 0) {
            newSelected = newSelected.concat(
                selected.slice(0, selectedIndex),
                selected.slice(selectedIndex + 1)
            );
        }
        else {
            newSelected = newSelected.concat(selected, name);
        }

        this.setState({ selected: newSelected });
    }

    private handlePageChange = (event: unknown, page: number) => {
        this.setState({ page });
    }

    private handleRowsPerPageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({ rowsPerPage: +event.target.value, page: 0 });
    }

    private isSelected = (name: string) => {
        const { selected } = this.state;
        return selected.indexOf(name) !== -1;
    }

    private stableSort = (array: any[], cmp: (left, right) => number) => {
        const stabilizied = array.map((o, index) => [o, index] as [any, number]);
        stabilizied.sort((left, right) => {
            const order = cmp(left, right);
            if (order !== 0) return order;
            return left[1] - right[1];
        });
        return stabilizied.map(o => o[0]);
    }

    private getSorting = (order: Order, orderBy: string): (a: any, b: any) => number => {
        return order === 'desc' ? (a, b) => this.desc(a, b, orderBy) : (a, b) => -this.desc(a, b, orderBy);
    }

    private desc = (a: any, b: any, orderBy: string) => {
        if (b[orderBy] < a[orderBy]) {
            return -1;
        }
        if (b[orderBy] > a[orderBy]) {
            return 1;
        }
        return 0;
    }

    render() {
        const { selected, order, orderBy, page, rowsPerPage } = this.state;
        const { classes, dense, data, title, columns, appendCheckbox } = this.props;
        const emptyRows = rowsPerPage - Math.min(rowsPerPage, data.length - page * rowsPerPage);
        return (
            <div className={classes.root}>
                {title && <TableToolbar text={title} selected={selected.length} />}
                <div className={classes.tableWrapper}>
                    <Table
                        className={classes.table}
                        aria-labelledby="tableTitle"
                        size={dense ? 'small' : 'medium'}
                    >
                        <MaterialTableHead
                            classes={classes}
                            selected={selected.length}
                            appendCheckBox={appendCheckbox}
                            order={order}
                            orderBy={orderBy}
                            columns={columns}
                            onSelectAllClick={this.handleSelectAllClick}
                            onSortColumnClick={this.handleSortColumnClick}
                            rowCount={data.length}
                        />
                        <TableBody>
                            {this.stableSort(data, this.getSorting(order, orderBy))
                                .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                                .map((row, index) => {
                                    const isItemSelected = this.isSelected(row.name);
                                    const labelId = `enhanced-table-checkbox-${index}`;

                                    const renderedCells = columns.map(column => (
                                        <TableCell id={labelId} padding="none" align={column.align}>
                                            {row[column.id]}
                                        </TableCell>
                                    ));

                                    return (
                                        <TableRow
                                            hover
                                            onClick={event => this.handleRowClick(event, row.name)}
                                            role="checkbox"
                                            aria-checked={isItemSelected}
                                            tabIndex={-1}
                                            key={row.name}
                                            selected={isItemSelected}
                                        >
                                            {appendCheckbox && (
                                                <TableCell padding="checkbox">
                                                    <Checkbox
                                                        checked={isItemSelected}
                                                        inputProps={{ 'aria-labelledby': labelId }}
                                                    />
                                                </TableCell>
                                            )}
                                            {renderedCells}
                                        </TableRow>
                                    );
                                })}
                            {emptyRows > 0 && (
                                <TableRow style={{ height: 49 * emptyRows }}>
                                    <TableCell colSpan={6} />
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </div>
                <TablePagination
                    rowsPerPageOptions={[5, 10, 25]}
                    component="div"
                    count={data.length}
                    rowsPerPage={rowsPerPage}
                    page={page}
                    backIconButtonProps={{
                        'aria-label': 'previous page',
                    }}
                    nextIconButtonProps={{
                        'aria-label': 'next page',
                    }}
                    onChangePage={this.handlePageChange}
                    onChangeRowsPerPage={this.handleRowsPerPageChange}
                />
            </div>
        );
    }
}

export const ReactTable = withStyles(styles)(ReactTableBase);