export interface Column {
    id: string;
    label: string;
    align: 'inherit' | 'left' | 'center' | 'right' | 'justify';
    disableSort: boolean;
    disablePadding: boolean;
    render?: (data: any) => JSX.Element;
    sort?: (left: any, right: any, orderBy: string) => number;
}

export enum Order {
    asc = 'asc',
    desc = 'desc'
}