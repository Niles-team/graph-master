export interface Route {
    name: string;
    path: string;
    iconName: string;
    exact?: boolean;
    component?: React.ComponentType;
}