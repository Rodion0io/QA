import type {Status} from "../@types/api";

import { THREE_DAYS_ON_MILSEC } from "./constants/constants";

import {STATUS_TASK} from "./constants/statusTask.ts";

export const checkStatusTask = (stringDate: string, status: Status): string => {
    const date = new Date(stringDate).getTime();
    const currentDate = new Date().getTime();

    // if (date - currentDate <= 0 && status === "ACTIVE"){
    //     return STATUS_TASK["missed"];
    // }
    // else if (date - currentDate <= THREE_DAYS_ON_MILSEC && status === "ACTIVE"){
    //     return STATUS_TASK["warrning"];
    // }
    // else if (status === "COMPLETED"){
    //     return STATUS_TASK["completed"]
    // }
    // else if (status === "LATE"){
    //     return STATUS_TASK["late"];
    // }
    // return STATUS_TASK["ok"];

    if (date - currentDate <= 0 && status === "OVERDUE"){
        return STATUS_TASK["missed"];
    }
    else if (date - currentDate <= THREE_DAYS_ON_MILSEC && status === "ACTIVE"){
        return STATUS_TASK["warrning"];
    }
    else if (status === "COMPLETED"){
        return STATUS_TASK["completed"]
    }
    else if (status === "LATE"){
        return STATUS_TASK["late"];
    }
    return STATUS_TASK["ok"];
}


// Если дедлайн уже настпул, то статус - Overdue
// Completed - если выпоонили вовремя
// Late - если выполинили после дейлика
// Задача переходит из Completed в Active если deadline не наступил.
// Задача переходит в Overdue, если deadline наступил.


// Срок пропущен, если у нас текущая дата - дата дейлика <= 0
//  и при этом у нас статус задачи активен, то мы красим ее в красный цвет

// Если текущая дата - дата дейлика <= три дня и статус активный, то красим в оранжевый цвет