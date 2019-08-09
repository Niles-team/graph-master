import * as React from "react";

import {
    withStyles,
    WithStyles,
    Grid,
    Paper,
    Avatar,
    Typography,
    TextField,
    FormControlLabel,
    Checkbox,
    Button,
    Link,
    CircularProgress
} from "@material-ui/core";
import { layoutStyles, authenticateStyles, commonStyles } from "../mui-theme";
import { Timeline } from "@material-ui/icons";
import { mergeStyles } from "../utils";
import { userService } from "../services";
import { AuthenticatedUser } from "../models";

const styles = mergeStyles(layoutStyles, authenticateStyles, commonStyles);

interface Props extends WithStyles<typeof styles> {

}

interface State {
    loading: boolean;
    message: string;
    userName: string;
    password: string;
    rememberMe?: boolean;
}

class SignInBase extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);

        this.state = {
            loading: false,
            message: '',
            userName: '',
            password: '',
            rememberMe: false
        };
    }

    private handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const { userName, password } = this.state;

        this.setState({
            loading: true
        });

        const result: AuthenticatedUser = await userService.signIn(userName, password);

        this.setState({
            loading: false,
            message: result.message,
            password: ''
        })
    }

    private handleUserNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            userName: event.target && event.target.value,
            message: ''
        });
    }

    private handlePasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            password: event.target && event.target.value,
            message: ''
        });
    }

    private handleRememberMeChange = (event: React.ChangeEvent<HTMLInputElement>, checked: boolean) => {
        this.setState({
            rememberMe: checked
        });
    }

    private validateCredentials(): boolean {
        const { userName, password } = this.state;
        return userName && userName.length >= 5 && password && password.length >= 8;
    }

    render() {
        const { classes } = this.props;
        const {
            loading,
            message,
            userName,
            password,
            rememberMe
        } = this.state;
        return (
            <Grid container component="main" className={classes.root}>
                <Grid item xs={false} sm={6} md={7} className={classes.image} />
                <Grid item xs={12} sm={6} md={5} component={Paper} elevation={6} square>
                    <Grid className={classes.paper}>
                        <Avatar className={classes.avatar}>
                            <Timeline />
                        </Avatar>
                        <Typography component="h1" variant="h5">
                            Sign in to Graph-Master
                        </Typography>
                        <form className={classes.form} onSubmit={this.handleSubmit}>
                            <TextField
                                variant="outlined"
                                margin="normal"
                                required
                                fullWidth
                                placeholder="Enter your user name"
                                id="userName"
                                label="User Name"
                                name="userName"
                                autoComplete="username"
                                value={userName}
                                onChange={this.handleUserNameChange}
                                autoFocus
                            />
                            <TextField
                                variant="outlined"
                                margin="normal"
                                required
                                fullWidth
                                name="password"
                                label="Password"
                                type="password"
                                id="password"
                                autoComplete="current-password"
                                value={password}
                                onChange={this.handlePasswordChange}
                            />
                            <FormControlLabel
                                control={<Checkbox value={rememberMe} onChange={this.handleRememberMeChange} color="primary" />}
                                label="Remember me"
                            />
                            <div className={classes.submit}>
                                <Button
                                    type="submit"
                                    fullWidth
                                    variant="contained"
                                    color="primary"
                                    disabled={loading || !this.validateCredentials()}
                                >
                                    <div>Sign In</div>
                                    {loading && <CircularProgress size={24} className={classes.buttonProgress} />}
                                </Button>
                                {message && <Typography variant="subtitle2" component="h6" color="error" className={classes.submitMessage}>{message}</Typography>}
                            </div>
                            <Grid container>
                                <Grid item xs>
                                    <Link href="/forgot-password" variant="body2">
                                        Forgot password?
                                    </Link>
                                </Grid>
                                <Grid item>
                                    <Link href="/sign-up" variant="body2">
                                        {"Don't have an account?"}
                                    </Link>
                                </Grid>
                            </Grid>
                        </form>
                    </Grid>
                </Grid>
            </Grid>
        );
    }
}

export const SignIn = withStyles(styles)(SignInBase);