import { deleteTask } from "../../../utils/api/deleteTask";

export const handleDelete = async (event: React.MouseEvent<HTMLImageElement>) => {
    const id = event.currentTarget.id;
    try{
        await deleteTask(id);
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