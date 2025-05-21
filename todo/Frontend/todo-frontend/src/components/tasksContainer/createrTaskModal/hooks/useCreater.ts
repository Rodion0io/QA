import { useState } from "react";

import { createTask } from "../../../../utils/api/createTask";

import type {CreateTaskRequest} from "../../../../@types/api";
import { DATE_MACROS } from "../../../../utils/constants/constants";

export const useCreater = (): [(input: string, value: string) => void,
    () => Promise<void>, boolean, string] => {

    const [newTask, setNewTask] = useState<CreateTaskRequest>({
        title: "",
        description: undefined,
        deadline: undefined,
        priority: undefined
    });

    const [errorFlag, setErrorFlag] = useState<boolean>(false);
    const [errorMessage, setErrorMessage] = useState<string>("");

    const handleChange = (input: string, value: string) => {
        setNewTask((prev) => (
            {...prev, [input]: value}
        ))
    }


    const handleClick = async () => {
        if (newTask.title.length < 4){
            setErrorFlag(true);
            setErrorMessage("Длина названия минимум 4 символа");
        }
        else if (newTask.title.length > 255){
            setErrorFlag(true);
            setErrorMessage("Максимальная длина названия максимум 255 символа");
        }
        else if (newTask.description && (newTask.description.length < 15 || newTask.description.length > 500)){
            setErrorFlag(true);
            setErrorMessage("Описание либо меньше 15 символов, либо больше 500");
        }
            // else if (!newTask.deadline && !DATE_MACROS.test(newTask.title)){
            //     setErrorFlag(true);
            //     setErrorMessage("Введите дату");
        // }
        // else if (
        //     (newTask.deadline && new Date(newTask.deadline) <= new Date()) ||
        //     (newTask.title.match(DATE_MACROS) && new Date(newTask.title.match(DATE_MACROS)![0]) <= new Date())
        // ){
        //     setErrorFlag(true);
        //     setErrorMessage("Вы не можете планировать на прошедшее время!");
        // }
        else{
            setErrorFlag(false);
            setErrorMessage("");
            try {
                console.log(newTask.title.match(DATE_MACROS))
                if (newTask.priority === ""){
                    newTask.priority = undefined
                }
                if (newTask.deadline){
                    newTask.deadline = new Date(newTask.deadline).toISOString();
                }
                if (newTask.deadline === undefined){
                    newTask.deadline = null;
                }
                await createTask(newTask);
                window.location.href = "/";
            } catch (error) {
                setErrorFlag(true);
                setErrorMessage(`${error}`);
                console.error(error)
            }
        }
    }

    return [handleChange, handleClick, errorFlag, errorMessage];
};