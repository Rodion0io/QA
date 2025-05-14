import {URL} from "../constants/constants";

import axios from "axios"

import { RequestPattern } from "./instances";

export const concreteTask = async (id: string) => {
    try{
        const response = await RequestPattern.delete(`${URL}getTask/${id}`,);
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