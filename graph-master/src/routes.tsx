import * as React from "react";
import { Switch, Redirect } from "react-router-dom";

import { routes as localRoutes } from "./sharedConstants";

import { ErrorRedirect } from "./components/common";
import { PrivateRoute, PublicRoute } from "./components/routeComponents";
import { SignIn } from "./components";
import { SignUp } from "./components";
import { Error as ErrorPage } from "./components";
import { EmailConfirm } from "./components";

const dashboard = localRoutes.find(o => o.name === 'Dashboard');

export const routes = (
    <ErrorRedirect>
        <Switch>
            {localRoutes.map(route => <PrivateRoute exact={route.exact} path={route.path} component={route.component} />)}
            <PublicRoute restricted={true} exact path="/sign-in" component={SignIn} />
            <PublicRoute restricted={true} exact path="/sign-up" component={SignUp} />
            <PublicRoute restricted={true} path="/confirm-email/" component={EmailConfirm} />
            <PublicRoute restricted={false} exact path="/error" component={ErrorPage} />
            <Redirect to={dashboard.path} />
        </Switch>
    </ErrorRedirect>
);
