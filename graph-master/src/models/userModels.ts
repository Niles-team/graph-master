export interface User {
    id: number;
    userName: string;
    password: string;
    firstName: string;
    lastName: string;
    email: string;
    teamId?: number;
}

export interface AuthenticatedUser extends User, UserResponse {
    token?: string;
}

export interface UserResponse {
    message?: string;
}