import { Route } from "../models";
import { Dashboard, Graphics } from "../components";

export const routes: Route[] = [
    { name: 'Dashboard', path: '/', iconName: 'Dashboard', exact: true, component: Dashboard },
    { name: 'Graphics', path: '/graphics', iconName: 'Timeline', exact: true, component: Graphics },
]