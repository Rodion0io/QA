import type {UpdateStatusDto} from "../../../@types/api";

import { updateStatus } from "../../../utils/api/updateStatus";

export const handleChangeStatus = async (
    event: React.MouseEvent<HTMLImageElement>) => {
    const id = event.currentTarget.id;
    const currentStatus = event.currentTarget.alt;
    let body: UpdateStatusDto = {status: "ACTIVE"}
    if (currentStatus === "ACTIVE"){
        body = {status: "COMPLETED"};
    }
    try{
        await updateStatus(id, body);
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