import { UnauthorizedError, ApplicationError } from "../models";

export type AssignFromSource = object | ((original: Record<any, any>) => object);
export type ResponseHandler<T> = (response: Response) => Promise<T>

export interface ResponseOptions {
    messageFormatter?: (message: string) => string;
}

export interface JsonResponseOptions extends ResponseOptions {
    dateProperties?: string[],
    assignFromSource?: AssignFromSource,
}

export async function handleResponse(response: Response, options?: ResponseOptions | undefined): Promise<void> {
    if (!response.ok) {
        await handleNotOkResponse(response, options && options.messageFormatter);
    }
}

export async function handleJsonResponse<T>(response: Response, options?: JsonResponseOptions | undefined): Promise<T> {
    if (response.ok) {
        let contentType = response.headers.get('content-type');
        if (response.status === 204) {
            return null;
        }
        if (contentType && contentType.includes('application/json')) {
            const text = await response.text();
            let value = JSON.parse(text, options ? createReviver(options) : undefined);
            if (options && options.assignFromSource) {
                assignFromSource(value, options.assignFromSource);
            }
            return value as T;
        } else {
            throw new ApplicationError(`Unexpected content type. Status: ${response.status} - ${response.statusText}`);
        }
    } else {
        await handleNotOkResponse(response, options && options.messageFormatter);
    }
}

export async function handleTextResponse(response: Response, options?: ResponseOptions | undefined): Promise<string> {
    if(response.ok) {
        return await response.text();
    } else {
        await handleNotOkResponse(response, options && options.messageFormatter);
    }
}

async function handleNotOkResponse(response: Response, messageFormatter: ((text: string) => string) | undefined): Promise<never> {
    if (response.status === 401) {
        throw new UnauthorizedError();
    }
    else {
        let text = await response.text();
        if (messageFormatter) {
            text = messageFormatter(text);
        }
        const message = `Error ${response.status}: ${text}`
        throw new ApplicationError(message);
    }
}


function createReviver(options: JsonResponseOptions) {
    const datePropertiesMap = options.dateProperties && options.dateProperties.reduce((map, property) => {
        map[property] = true;
        return map;
    }, {});
    return options.dateProperties && datePropertiesMap ?
        (key: any, value: any) => {
            if (datePropertiesMap[key]) {
                value = value ?
                    new Date(value) :
                    null;
            }
            return value;
        } : undefined;
}

function assignFromSource(value: any, source: AssignFromSource) {
    if (value instanceof Array) {
        for (let i = 0; i < value.length; i++) {
            const item = value[i];
            if (typeof item === 'object') {
                Object.assign(item, typeof source === 'object' ? source : source(item));
            }
        }
    }
    else if (typeof value === 'object') {
        Object.assign(value, typeof source === 'object' ? source : source(value));
    }
}