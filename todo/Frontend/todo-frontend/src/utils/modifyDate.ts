export const modifyDate = (date: string): string => {
    let firstPattern = date.split(".")[0];
    let secondPattern = firstPattern.split("T");
    let result = "";
    result += secondPattern[0].split("-").reverse().join("-") + " " + secondPattern[1].slice(0,5);
    return result;
};