import { handleJsonResponse, ResponseHandler } from "../utils";
import { AuthenticatedUser, User } from "../models";

class UserService {
    public async signIn(userName: string, password: string) {
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

    public async signUp(userName: string, password: string, firstName: string, lastName: string, email: string) {
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
}

export const userService = new UserService();