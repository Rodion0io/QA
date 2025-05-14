import { URL } from "../constants/constants";

import axios from "axios"

import { RequestPattern } from "./instances";

import type {CreateTaskRequest} from "../../@types/api";

export const updateCard = async (id: string, body: CreateTaskRequest) => {
    try{
        const response = await RequestPattern.put(`${URL}update/${id}`, body);
        return response.data;
    }
    catch (error){
        if (axios.isAxiosError(error)) {
            console.log(error.message);
            return error.message;
        }
        // throw new Error(error);
    }
}