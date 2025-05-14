export type Status = "ACTIVE" | "COMPLETED" | "OVERDUE" | "LATE";

export type Priority = "" | "LOW" | "MEDIUM" | "HIGH" | "CRITICAL";

export type SortType = "" | "CreateAsc" | "CreateDesc" | "PriorityAsc" | "PriorityDesc" | "StatusAsc" | "StatusDesc"

export type SelectionPrioroty = Priority | "";

export type SelectionStatus = Status | "";

export type SelectionSort = SortType | "";

export interface SortParams{
    priority?: string,
    sort?: string
}

export interface Task{
    id: string,
    title: string,
    description: string,
    deadline?: string,
    status: Status,
    priority: Priority,
    updatedTime?: string
}

export interface CreateTaskRequest{
    title: string,
    description?: string,
    deadline?: string | null,
    priority?: Priority
}

export interface UpdateStatusDto{
    status: Status
}