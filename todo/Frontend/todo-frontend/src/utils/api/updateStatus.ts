import { URL } from "../constants/constants";

import axios from "axios"

import { RequestPattern } from "./instances";

import type {UpdateStatusDto} from "../../@types/api";

export const updateStatus = async (id: string, body: UpdateStatusDto) => {
    try{
        const response = await RequestPattern.put(`${URL}updateStatus/${id}`, body);
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