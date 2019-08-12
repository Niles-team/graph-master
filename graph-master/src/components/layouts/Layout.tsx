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
    Grid,
} from "@material-ui/core";
import { withStyles } from "@material-ui/core";
import useScrollTrigger from '@material-ui/core/useScrollTrigger';
import { WithStyles } from "@material-ui/styles";

import { HamburgerArrowTurn } from "react-animated-burgers";

import { routes } from "../../sharedConstants";
import { mergeStyles } from "../../utils";
import { layoutStyles } from "../../mui-theme";
import { Dashboard, Timeline, ExitToApp } from "@material-ui/icons";
import { sessionService } from "../../services";

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

class LayoutBase extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);

        this.state = {
            drawerOpen: false,
        }
    }

    private signOut = () => {
        const { history } = this.props;
        sessionService.signOut();
        history.push('/sign-in');
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
                    <AppBar position="absolute" className={classes.appBar}>
                        <Toolbar className={classes.toolbar}>
                            <HamburgerArrowTurn  buttonWidth={24} isActive={drawerOpen} toggleButton={this.toggleDrawerState} barColor="white" className={classes.hamburgerButtonStyle} />
                            <Typography component="h1" variant="h6" color="inherit" noWrap>
                                {currentRoute.name}
                            </Typography>
                        </Toolbar>
                    </AppBar>
                <Drawer
                    variant="permanent"
                    classes={{
                        paper: `${classes.drawerPaper} ${!drawerOpen && classes.drawerPaperClose}`,
                    }}
                    open={drawerOpen}
                >
                    <Grid container direction="column" className={classes.root}>
                        <Grid item>
                            <List>
                                <div className={classes.appBarSpacer} />
                                {menuItems}
                            </List>
                        </Grid>
                        <Grid item xs/>
                        <Grid item>
                            <List>
                                <ListItem key="signOut" button onClick={this.signOut}>
                                    <ListItemIcon>{!drawerOpen ?
                                        (
                                            <Tooltip title={"Sign out"}>
                                                <ExitToApp/>    
                                            </Tooltip>
                                        ) : <ExitToApp/>
                                    }
                                    </ListItemIcon>
                                    <ListItemText primary={"Sign out"} />
                                </ListItem>
                            </List>
                        </Grid>
                    </Grid>
                </Drawer>
                <div className={classes.content}>
                    <div className={classes.appBarSpacer} />
                    <Container className={classes.container}>
                        {children}
                    </Container>
                </div>
            </div>
        );
    }
}

export const Layout = withStyles(styles)(withRouter(LayoutBase));