import { SvgIconProps } from "@material-ui/core/SvgIcon";

export interface Route {
    name: string;
    path: string;
    iconName?: string;
    component: React.ComponentType;
    exact?: boolean;
}