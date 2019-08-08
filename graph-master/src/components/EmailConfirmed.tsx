import * as React from "react";
import { RouteComponentProps } from "react-router";

import { withStyles, WithStyles } from "@material-ui/styles";

import { mergeStyles } from "../utils";
import { userService } from "../services";
import { Grid, Typography, Link, Button, CircularProgress } from "@material-ui/core";
import { authenticateStyles, layoutStyles } from "../mui-theme";

const styles = mergeStyles(authenticateStyles, layoutStyles);

interface Props extends RouteComponentProps, WithStyles<typeof styles> {

}

interface State {
    firstName: string;
    lastName: string;
    userConfirmed: boolean;
    loading: boolean;
}

class EmailConfirmedBase extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);

        this.state = {
            firstName: '',
            lastName: '',
            userConfirmed: false,
            loading: false
        }
    }

    private getConfirmCode(url: string): string {
        if (!Boolean(url)) {
            return '';
        }

        const address = '/confirm-email/';
        const code = url.slice(address.length);

        return code;
    }

    async componentDidMount() {
        const { location } = this.props;
        const confirmCode = this.getConfirmCode(location.pathname);
        this.setState({ loading: true });

        const user = await userService.confirmUser(confirmCode);

        this.setState({
            firstName: user && user.firstName,
            lastName: user && user.lastName,
            userConfirmed: Boolean(user),
            loading: false
        });
    }

    private handleSubmit = () => {
        const { history } = this.props;
        history.push('/sign-in');
    }

    render() {
        const {
            classes
        } = this.props;
        const {
            firstName,
            lastName,
            userConfirmed,
            loading
        } = this.state;

        let content;
        if(loading) {
            content = <CircularProgress className={classes.buttonProgress} />;
        } 
        else if (userConfirmed) {
            content = (
                <Grid item>
                    <Typography variant="h5" component="h5">Hi {firstName} {lastName}!</Typography>
                    <Typography variant="subtitle1" component="h6">
                        We glad to see you are using Graph Master!
                    </Typography>

                    <div className={classes.submit}>
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            color="primary"
                            onClick={this.handleSubmit}
                        >
                            Go to sign in
                        </Button>
                    </div>
                </Grid>
            )
        }
        else {
            content = (
                <Grid item>
                    <Typography variant="h5" component="h5">Oh no! We can't find that confirm code...</Typography>
                    <Typography variant="subtitle1" component="h6">
                        You can try to create another account. Maybe you already confirmed your account.
                    </Typography>
                    <Grid container>
                        <Grid item xs>
                            <Link href="/sign-up" variant="body2">
                                Try to create another account
                            </Link>
                        </Grid>
                        <Grid item>
                            <Link href="/sign-in" variant="body2">
                                Try to sign in
                            </Link>
                        </Grid>
                    </Grid>
                </Grid>
            );
        }

        return (
            <Grid container direction="row" alignItems="center" justify="center" className={classes.root}>
                <Grid item xs />
                {content}
                <Grid item xs />
            </Grid>
        );
    }
}

export const EmailConfirm = withStyles(styles)(EmailConfirmedBase);