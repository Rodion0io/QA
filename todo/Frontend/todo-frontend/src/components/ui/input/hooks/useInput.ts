import { useState } from "react"

export const useInput = (initialState: string,
                         changer?:(value: string) => void):
    [string, (event: React.ChangeEvent<HTMLInputElement>) => void] => {

    const [inputValue, setInputValue] = useState(initialState);

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const value = event.target.value;

        setInputValue(value);

        if (changer){
            changer(value);
        }
    }

    return [inputValue, handleChange];
}