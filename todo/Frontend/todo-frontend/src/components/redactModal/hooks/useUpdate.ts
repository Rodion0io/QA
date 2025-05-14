import { useState } from "react";

import type {CreateTaskRequest, Task} from "../../../@types/api";

import { updateCard } from "../../../utils/api/updateCard";

export const useUpdate = (currentValuesTask: Task,id: string): [(input: string, value: string) => void,
    () => Promise<void>, boolean, string] => {

    const [newTask, setNewTask] = useState<CreateTaskRequest>({
        title: currentValuesTask.title,
        description: currentValuesTask.description,
        deadline: currentValuesTask.deadline,
        priority: currentValuesTask.priority
    });

    const [errorFlag, setErrorFlag] = useState<boolean>(false);
    const [errorMessage, setErrorMessage] = useState<string>("");

    const handleChange = (input: string, value: string) => {
        setNewTask((prev) => (
            {...prev, [input]: value}
        ))
    }

    const handleClick = async () => {
        // const id = event.currentTarget.id;
        if (newTask.title.length < 4){
            setErrorFlag(true);
            setErrorMessage("Длина названия минимум 4 символа");
        }
        else if (newTask.description && (newTask.description.length < 15 || newTask.description.length > 500)){
            setErrorFlag(true);
            setErrorMessage("Описание либо меньше 15 символов, либо больше 500");
        }
            // else if (!newTask.deadline && !DATE_MACROS.test(newTask.title)){
            //     setErrorFlag(true);
            //     setErrorMessage("Введите дату");
        // }
        else if (newTask.deadline && new Date(newTask.deadline) <= new Date()){
            setErrorFlag(true);
            setErrorMessage("Вы не можете планировать на прошедшее время!");
        }

        else{
            setErrorFlag(false);
            setErrorMessage("");
            try {
                if (newTask.priority === ""){
                    newTask.priority = undefined
                }
                if (newTask.deadline){
                    newTask.deadline = new Date(newTask.deadline).toISOString();
                }
                if (newTask.deadline === undefined){
                    newTask.deadline = null;
                }
                await updateCard(id, newTask);
                // window.location.href = "/";
            } catch (error) {
                setErrorFlag(true);
                setErrorMessage(`${error}`);
                console.error(error)
            }
        }
    }

    return [handleChange, handleClick, errorFlag, errorMessage];
};