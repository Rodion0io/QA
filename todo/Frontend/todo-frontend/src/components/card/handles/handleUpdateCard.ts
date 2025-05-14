import type {CreateTaskRequest} from "../../../@types/api";

import { updateCard } from "../../../utils/api/updateCard";

export const handleUpdateCard = async (id: string, body: CreateTaskRequest) => {
    try{
        await updateCard(id, body);
        window.location.href = "/";
    }
    catch (error){
        if (error instanceof Error) {
            throw new Error(error.message);
        } else {
            throw new Error(String(error));
        }
    }
}