import * as React from "react";
import { Switch, Route, Redirect } from "react-router-dom";

import { UnauthorizedRedirect } from "./components/common";
import { Layout } from "./components/layouts";
import { routes as localRoutes } from "./sharedConstants";

const dashboard = localRoutes.find(o=> o.name === 'Dashboard');

export const routes = (
    <UnauthorizedRedirect>
        <Layout>
            <Switch>
                {localRoutes.map(route => <Route exact={route.exact} path={route.path} component={route.component} />)}
                <Redirect to={dashboard.path} />
            </Switch>
        </Layout>
    </UnauthorizedRedirect>
);
