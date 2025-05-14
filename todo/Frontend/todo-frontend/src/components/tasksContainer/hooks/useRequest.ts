import { useState, useEffect } from "react";

import type {Task} from "../../../@types/api";

import { listTask } from "../../../utils/api/listTask";

interface useRequestProp{
    urlPattern: string
}

export const useRequest = ({ urlPattern }: useRequestProp): Task[] => {
    const [tasks, setTasks] = useState<Task[]>([]);

    useEffect(() => {
        const request = async () => {
            const response = await listTask(urlPattern);
            setTasks(response);
        }
        request();
    }, [urlPattern]);

    return tasks;
}