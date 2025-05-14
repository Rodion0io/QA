import type {SortParams} from "../@types/api";

export const createUrlPattern = (datas: SortParams): string => {

    let result = `?${datas.priority ? `priority=${datas.priority}` : ""}${datas.priority && datas.sort ? "&" : ""}${datas.sort ? `sort=${datas.sort}` : ""}`;

    return result;
}

//Более крутой вариант
// export const createUrlPattern = (datas: SortParams): string => {
//     const params = [
//       datas.priority && `priority=${datas.priority}`,
//       datas.sort && `sort=${datas.sort}`
//     ].filter(Boolean); // убираем пустые

//     return params.length ? `?${params.join("&")}` : "";
//   }
  