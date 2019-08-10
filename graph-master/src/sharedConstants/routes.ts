import { Route } from "../models";
import { Dashboard, Graphs } from "../components";

export const routes: Route[] = [
    { name: 'Dashboard', path: '/', iconName: 'Dashboard', exact: true, component: Dashboard },
    { name: 'Graphs', path: '/graphs', iconName: 'Timeline', exact: true, component: Graphs },
]