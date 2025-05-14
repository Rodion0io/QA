import axios from "axios"

import { URL } from "../constants/constants";

import { RequestPattern } from "./instances";

import type {Task} from "../../@types/api";



export const listTask = async (urlPatter: string): Promise<Task[]> => {
    try{
        const response = await RequestPattern.get<Task[]>(`${URL}tasks/${urlPatter}`);
        return response.data;
    }
    catch (error){
        if (axios.isAxiosError(error)) {
            console.log(error.message);
        }
        throw new Error("Произошла ошибка редактирования поста или вынесения вердикта");
    }
}