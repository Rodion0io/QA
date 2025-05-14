import { useRef, useState } from "react";

import { createUrlPattern } from "../../../utils/createUrlPattern";

import type {SortParams} from "../../../@types/api";

export const useFilter = (): [(input: string, value: string) => void, React.RefObject<string>] => {

    const [urlPart, setUrlPart] = useState<SortParams>({priority: undefined, sort: undefined});
    let pattern = useRef("");

    const handleChange = (input: string, value: string) => {
        setUrlPart((prev) => (
            {...prev, [input]: value}
        ))
    }

    pattern.current = createUrlPattern(urlPart)


    // const handleClick = (): string => {
    //     const pattern = createUrlPattern(urlPart);
    //     return pattern
    // }

    return [handleChange, pattern];
}