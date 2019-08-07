import * as React from "react";
import { Switch, Redirect } from "react-router-dom";

import { ErrorRedirect } from "./components/common";
import { routes as localRoutes } from "./sharedConstants";
import { PrivateRoute, PublicRoute } from "./components/routeComponents";
import { SignIn } from "./components/SignIn";
import { SignUp } from "./components/SignUp";
import { Error as ErrorPage } from "./components/Error";

const dashboard = localRoutes.find(o => o.name === 'Dashboard');

export const routes = (
    <ErrorRedirect>
        <Switch>
            {localRoutes.map(route => <PrivateRoute exact={route.exact} path={route.path} component={route.component} />)}
            <PublicRoute restricted={true} exact path="/sign-in" component={SignIn} />
            <PublicRoute restricted={true} exact path="/sign-up" component={SignUp} />
            <PublicRoute restricted={false} exact path="/error" component={ErrorPage} />
            <Redirect to={dashboard.path} />
        </Switch>
    </ErrorRedirect>
);
