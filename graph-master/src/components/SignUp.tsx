import * as React from "react";

import { WithStyles, withStyles } from "@material-ui/styles";

import { mergeStyles } from "../utils";
import { authenticateStyles, commonStyles, layoutStyles } from "../mui-theme";
import { Grid, Avatar, Paper, TextField, Typography, FormControlLabel, Button, CircularProgress, Link, Checkbox, FormControl, FormHelperText } from "@material-ui/core";
import { Timeline } from "@material-ui/icons";
import { User, UserResponse } from "../models";
import { userService } from "../services";

const styles = mergeStyles(layoutStyles, authenticateStyles, commonStyles);

interface Props extends WithStyles<typeof styles> { }

interface State {
    userName: string;
    userNameError?: string;
    password: string;
    passwordError?: string;
    firstName: string;
    lastName: string;
    email: string;
    emailError?: string;
    message: string;
    loading: boolean;
}

class SignUpBase extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);

        this.state = {
            userName: '',
            password: '',
            firstName: '',
            lastName: '',
            email: '',
            message: '',
            loading: false
        }
    }

    private handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const { userName, password, firstName, lastName, email } = this.state;

        this.setState({
            loading: true
        });

        const result: User & UserResponse = await userService.signUp(userName, password, firstName, lastName, email);

        this.setState({
            loading: false,
            message: result.message,
            password: ''
        })
    }

    private handleUserNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            userName: event.target && event.target.value
        });
    }

    private handlePasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            password: event.target && event.target.value
        });
    }

    private handleFirstNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            firstName: event.target && event.target.value
        });
    }

    private handleLastNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            lastName: event.target && event.target.value
        });
    }

    private handleEmailChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            email: event.target && event.target.value
        });
    }

    private validateUserName = async () => {
        const { userName } = this.state;
        const userNameError = await userService.validateUserName(userName);
        this.setState({ userNameError });
    }

    private validateEmail = async () => {
        const { email } = this.state;
        const emailError = await userService.validateEmail(email);
        this.setState({ emailError });
    }

    private validatePassword = () => {
        const { password } = this.state;
        const passwordError = password.length < 8 ? 'Password is to short' : '';
        this.setState({ passwordError });
    }

    private validateCredentials(): boolean {
        const { userNameError, passwordError, firstName, lastName, emailError } = this.state;
        return !Boolean(userNameError)
            && !Boolean(passwordError)
            && !Boolean(emailError)
            && firstName && firstName.length > 0
            && lastName && lastName.length > 0;
    }

    render() {
        const { classes } = this.props;
        const {
            userName,
            userNameError,
            password,
            passwordError,
            firstName,
            lastName,
            email,
            emailError,
            message,
            loading
        } = this.state;

        const isCredentialsValid = this.validateCredentials();

        return (
            <Grid container component="main" className={classes.root}>
                <Grid item xs={false} sm={6} md={7} className={classes.image} />
                <Grid item xs={12} sm={6} md={5} component={Paper} elevation={6} square className={classes.paperContainer}>
                    <Grid className={classes.paper}>
                        <Avatar className={classes.avatar}>
                            <Timeline />
                        </Avatar>
                        <Typography component="h1" variant="h5">
                            Sign in to Graph-Master
                        </Typography>
                        <form className={classes.form} onSubmit={this.handleSubmit}>
                            <FormControl fullWidth error={Boolean(userNameError)}>
                                <TextField
                                    variant="outlined"
                                    margin="normal"
                                    required
                                    placeholder="Enter your user name"
                                    id="userName"
                                    label="User Name"
                                    name="userName"
                                    autoComplete="username"
                                    value={userName}
                                    onChange={this.handleUserNameChange}
                                    onBlur={this.validateUserName}
                                    autoFocus
                                />
                                {Boolean(userNameError) && <FormHelperText>{userNameError}</FormHelperText>}
                            </FormControl>
                            <FormControl fullWidth error={Boolean(passwordError)}>
                                <TextField
                                    variant="outlined"
                                    margin="normal"
                                    required
                                    name="password"
                                    label="Password"
                                    type="password"
                                    id="password"
                                    autoComplete="new-password"
                                    value={password}
                                    onChange={this.handlePasswordChange}
                                    onBlur={this.validatePassword}
                                />
                                {Boolean(passwordError) && <FormHelperText>{passwordError}</FormHelperText>}
                            </FormControl>
                            <FormControl fullWidth error={!Boolean(firstName)}>
                                <TextField
                                    variant="outlined"
                                    margin="normal"
                                    required
                                    fullWidth
                                    name="firstname"
                                    label="First name"
                                    id="firstname"
                                    autoComplete="given-name"
                                    value={firstName}
                                    onChange={this.handleFirstNameChange}
                                />
                                {!Boolean(firstName) && <FormHelperText>First name is required</FormHelperText>}
                            </FormControl>
                            <FormControl fullWidth error={!Boolean(lastName)}>
                                <TextField
                                    variant="outlined"
                                    margin="normal"
                                    required
                                    fullWidth
                                    name="lastname"
                                    label="Last name"
                                    id="lastname"
                                    autoComplete="family-name"
                                    value={lastName}
                                    onChange={this.handleLastNameChange}
                                />
                                {!Boolean(lastName) && <FormHelperText>Last name is required</FormHelperText>}
                            </FormControl>
                            <FormControl fullWidth error={Boolean(emailError)}>
                                <TextField
                                    variant="outlined"
                                    margin="normal"
                                    required
                                    fullWidth
                                    name="email"
                                    label="E-mail address"
                                    id="email"
                                    autoComplete="email"
                                    value={email}
                                    onChange={this.handleEmailChange}
                                    onBlur={this.validateEmail}
                                />
                                {Boolean(emailError) && <FormHelperText>{emailError}</FormHelperText>}
                            </FormControl>
                            <div className={classes.submit}>
                                <Button
                                    type="submit"
                                    fullWidth
                                    variant="contained"
                                    color="primary"
                                    disabled={loading || !isCredentialsValid}
                                >
                                    <div>Sign In</div>
                                    {loading && <CircularProgress size={24} className={classes.buttonProgress} />}
                                </Button>
                                {message && <Typography variant="subtitle2" component="h6" color="error" className={classes.submitMessage}>{message}</Typography>}
                            </div>
                            <Grid container>
                                <Grid item xs />
                                <Grid item>
                                    <Link href="/sign-in" variant="body2">
                                        {"Already have an account?"}
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

export const SignUp = withStyles(styles)(SignUpBase);