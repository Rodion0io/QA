import { URL } from "../constants/constants";

import axios from "axios"

import { RequestPattern } from "./instances";

import type {CreateTaskRequest, Task} from "../../@types/api";

export const createTask = async (body: CreateTaskRequest) => {
    try{
        const response = await RequestPattern.post<Task[]>(`${URL}create`, body);
        return response.data;
    }
    catch (error){
        if (axios.isAxiosError(error)) {
            console.log(error.message);
            return error.message;
        }
        if (error instanceof Error) {
            throw new Error(error.message);
        } else {
            throw new Error(String(error));
        }
    }
}