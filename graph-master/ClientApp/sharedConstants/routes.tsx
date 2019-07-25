import * as React from "react";

import { Dashboard } from "~/components/Dashboard";
import { Route } from "~/models/CommonModels";

const localRoutes: Route[] = [
    { name: 'Dashboard', iconName: 'Dashboard', component: Dashboard, path: '/', exact: true }
];

export default localRoutes;