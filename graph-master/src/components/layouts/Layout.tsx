import * as React from "react";
import * as PropTypes from 'prop-types';
import { withRouter, RouteComponentProps } from "react-router-dom";

import {
    AppBar,
    Toolbar,
    Typography,
    Drawer,
    List,
    ListItem,
    ListItemText,
    ListItemIcon,
    Container,
    Tooltip,
} from "@material-ui/core";
import { withStyles } from "@material-ui/core";
import useScrollTrigger from '@material-ui/core/useScrollTrigger';
import { WithStyles } from "@material-ui/styles";

import { HamburgerArrowTurn } from "react-animated-burgers";

import { routes } from "../../sharedConstants";
import { mergeStyles } from "../../utils";
import { layoutStyles } from "../../mui-theme";
import { Dashboard, Timeline } from "@material-ui/icons";

const controlStyles = {
    hamburgerButtonStyle: {
        padding: '12px',
        "&&:focus": {
            outline: 'none'
        }
    }
};

const styles = mergeStyles(controlStyles, layoutStyles);

interface Props extends WithStyles<typeof styles>, RouteComponentProps {
    window?: () => Window;
    children: React.ReactElement;
}

interface State {
    drawerOpen: boolean;
}

function ElevationScroll(props: Props) {
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

class LayoutBase extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);

        this.state = {
            drawerOpen: false,
        }
    }

    private toggleDrawerState = () => {
        const { drawerOpen } = this.state;
        this.setState({
            drawerOpen: !drawerOpen
        });
    }

    render() {
        const { classes, children, history } = this.props;
        const { drawerOpen } = this.state;

        const currentPath = history.location && history.location.pathname;

        const currentRoute = routes.find(o => o.path.includes(currentPath));

        const menuItems = routes.map(route => {
            let icon;
            switch (route.iconName) {
                case 'Dashboard': {
                    icon = <Dashboard />;
                    break;
                }
                case 'Timeline': {
                    icon = <Timeline />;
                    break;
                }
            }
            return (
                <ListItem key={route.name} button onClick={() => { history.push(route.path) }}>
                    <ListItemIcon>{!drawerOpen ?
                        (
                            <Tooltip title={route.name}>
                                {icon}
                            </Tooltip>
                        ) : icon
                    }
                    </ListItemIcon>
                    <ListItemText primary={route.name} />
                </ListItem>
            );
        });

        return (
            <div className={classes.root}>
                <ElevationScroll {...this.props}>
                    <AppBar position="fixed" className={classes.appBar}>
                        <Toolbar>
                            <HamburgerArrowTurn buttonWidth={24} isActive={drawerOpen} toggleButton={this.toggleDrawerState} barColor="white" className={classes.hamburgerButtonStyle} />
                            <Typography component="h1" variant="h6" color="inherit" noWrap>
                                {currentRoute.name}
                            </Typography>
                        </Toolbar>
                    </AppBar>
                </ElevationScroll>
                <Drawer
                    variant="permanent"
                    classes={{
                        paper: `${classes.drawerPaper} ${!drawerOpen && classes.drawerPaperClose}`,
                    }}
                    open={drawerOpen}
                >
                    <List>
                        <div className={classes.appBarSpacer} />
                        {menuItems}
                    </List>
                </Drawer>
                <div className={classes.content}>
                    <div className={classes.appBarSpacer} />
                    <Container>
                        {children}
                    </Container>
                </div>
            </div>
        );
    }
}

export const Layout = withStyles(styles)(withRouter(LayoutBase));