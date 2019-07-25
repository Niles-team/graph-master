import * as React from 'react';
import * as PropTypes from 'prop-types';

import {
    AppBar,
    Toolbar,
    Typography,
    Drawer,
    List,
    ListItem,
    ListItemIcon,
    ListItemText,
} from "@material-ui/core";
import { withStyles, WithStyles } from "@material-ui/core/styles";
import useScrollTrigger from '@material-ui/core/useScrollTrigger';

import { HamburgerArrowTurn } from "react-animated-burgers";

import localRoutes from "~/sharedConstants/routes";
import mergeStyles from "../utils/mergeStyles";
import { layoutStyle } from "~/mui-theme";
import { Dashboard as DashboardIcon } from '@material-ui/icons';

const controlStyles = {
    hamburgerButtonStyle: {
        padding: '12px',
        "&&:focus": {
            outline: 'none'
        }
    }
};

const styles = mergeStyles(controlStyles, layoutStyle);

export interface LayoutProps extends WithStyles<typeof styles> {
    window?: () => Window;
    children: React.ReactElement;
}

export interface LayoutState {
    drawerOpen: boolean;
}

function ElevationScroll(props: LayoutProps) {
    const { children, window } = props;
    // Note that you normally won't need to set the window ref as useScrollTrigger
    // will default to window.
    // This is only being set here because the demo is in an iframe.
    const trigger = useScrollTrigger({
        disableHysteresis: true,
        threshold: 0,
        target: window ? window() : undefined,
    });

    return React.cloneElement(children, {
        elevation: trigger ? 4 : 0,
    });
}

ElevationScroll.propTypes = {
    children: PropTypes.node.isRequired,
    // Injected by the documentation to work in an iframe.
    // You won't need it on your project.
    window: PropTypes.func,
};

class LayoutBase extends React.Component<LayoutProps, LayoutState> {
    constructor(props) {
        super(props);

        this.state = {
            drawerOpen: false
        };
    }

    private toggleDrawerState = () => {
        const { drawerOpen } = this.state;
        this.setState({
            drawerOpen: !drawerOpen
        });
    }

    public render() {
        const { classes, children } = this.props;
        const { drawerOpen } = this.state;

        const listItems: JSX.Element[] = localRoutes.map(route => {
            let icon = null;
            switch (route.iconName) {
                case 'Dashboard': {
                    icon = <DashboardIcon />
                }
            }
            return (
                <ListItem button>
                    <ListItemIcon>{icon}</ListItemIcon>
                    <ListItemText>{route.name}</ListItemText>
                </ListItem>
            );
        });

        return (
            <div>
                <ElevationScroll {...this.props}>
                    <AppBar color="primary" className={classes.appBar}>
                        <Toolbar>
                            <HamburgerArrowTurn buttonWidth={24} isActive={drawerOpen} toggleButton={this.toggleDrawerState} barColor="white" className={classes.hamburgerButtonStyle} />
                            <Typography component="h1" variant="h6" color="inherit">
                                Dashboard
                            </Typography>
                        </Toolbar>
                    </AppBar>
                </ElevationScroll>
                <Drawer
                    variant="permanent"
                    open={drawerOpen}
                    className={`${classes.drawer} ${drawerOpen ? classes.drawerOpen : classes.drawerClose}`}
                    classes={{
                        paper: `${classes.drawer} ${drawerOpen ? classes.drawerOpen : classes.drawerClose}`
                    }}
                >
                    <List className={classes.toolbar}>
                        {listItems}
                    </List>
                </Drawer>
                <div className={`${classes.mainContainer} ${drawerOpen ? classes.mainContainerClosed : classes.mainContainerOpened }`}>
                    {children}
                </div>
            </div>
        );
    }
}

export const Layout = withStyles(styles)(LayoutBase);