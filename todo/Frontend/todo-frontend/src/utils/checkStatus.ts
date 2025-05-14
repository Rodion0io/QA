export const checkStatus = (status: string): boolean => {
    if (status === "ACTIVE" || status === "OVERDUE"){
        return true;
    }
    return false;
}