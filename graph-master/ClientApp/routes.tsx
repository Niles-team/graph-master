import * as React from "react";
import { UnauthorizedRedirect } from "./components/common/UnauthorizeRedirect";
import { Route, Switch, Redirect } from "react-router";

import { Layout } from "./layouts/Layout";
import localRoutes from "./sharedConstants/routes";

const DashboardRoute = localRoutes[0];

export const routes = (
    <UnauthorizedRedirect>
        <Layout>
            <Switch>
                {localRoutes.map(o => {
                    return (
                        <Route component={o.component} path={o.path} exact={o.exact}></Route>
                    );
                })}
                <Redirect to={DashboardRoute.path} ></Redirect>                
            </Switch>
        </Layout>
    </UnauthorizedRedirect>
)