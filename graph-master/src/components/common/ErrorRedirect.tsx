import * as React from "react";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { UnauthorizedError, ApplicationError } from "../../models";

class ErrorRedirectBase extends React.Component<RouteComponentProps> {
    componentDidCatch(error) {
        if (error instanceof ApplicationError) {
            this.props.history.push('/error', { message: error.message });
        }
        if (error instanceof UnauthorizedError) {
            this.props.history.push('/sign-in');
        } else {
            throw error;
        }
    }

    render() {
        return this.props.children;
    }
}

export const ErrorRedirect = withRouter(ErrorRedirectBase);