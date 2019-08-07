import * as React from "react";
import { Route, Redirect } from "react-router";

import { sessionService } from "../../services";

export const PublicRoute = ({component: Component, restricted, ...rest}) => {
    return (
        // restricted = false meaning public route
        // restricted = true meaning restricted route
        <Route {...rest} render={props => (
            sessionService.isUserAuthenticated() && restricted ?
                <Redirect to="/" />
            : <Component {...props} />
        )} />
    );
};