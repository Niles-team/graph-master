import { handleJsonResponse, ResponseHandler } from "../utils";
import { AuthenticatedUser, User } from "../models";

class UserService {
    public async signIn(userName: string, password: string): Promise<AuthenticatedUser> {
        return fetch('api/users/sign-in', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ userName, password })
        })
            .then(handleJsonResponse as ResponseHandler<AuthenticatedUser>);
    }

    public async signUp(userName: string, password: string, firstName: string, lastName: string, email: string): Promise<User> {
        return fetch('api/users/sign-up', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                userName,
                password,
                firstName,
                lastName,
                email
            })
        })
            .then(handleJsonResponse as ResponseHandler<User>);
    }

    public async confirmUser(confirmCode: string): Promise<User> {
        return fetch('api/users/confirm-user', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ code: confirmCode })
        })
            .then(handleJsonResponse as ResponseHandler<User>);
    }
}

export const userService = new UserService();