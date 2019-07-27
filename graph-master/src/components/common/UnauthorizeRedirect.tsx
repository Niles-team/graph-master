import * as React from "react";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { UnauthorizedError } from "../../models";

class UnauthorizedRedirectBase extends React.Component<RouteComponentProps> {
    componentDidCatch(error) {
        if (error instanceof UnauthorizedError) {
            this.props.history.push('/');
        } else {
            throw error;
        }
    }

    render() {
        return this.props.children;
    }
}

export const UnauthorizedRedirect = withRouter(UnauthorizedRedirectBase);