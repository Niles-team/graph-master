import * as React from "react";
import { Route, Redirect } from "react-router";

import { sessionService } from "../../services";
import { Layout } from "../layouts";

export const PrivateRoute = ({component: Component, ...rest}) => {
    return (
        // Show the component only when the user is logged in
        // Otherwise, redirect the user to /signin page
        <Route {...rest} render={props => (
            sessionService.isUserAuthenticated() ?
                <Layout><Component {...props} /></Layout>
            : <Redirect to="/sign-in" />
        )} />
    );
};