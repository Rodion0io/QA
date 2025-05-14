import { useState } from "react";

export const useSelect = (initialState: string,
                          changer?:(value: string) => void)
    :[string, (event: React.ChangeEvent<HTMLSelectElement>) => void] => {

    const [selected, setSelected] = useState(initialState);

    const handleSelect = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const value = event.target.value;

        setSelected(value);

        if (changer){
            changer(value);
        }
    };

    return [selected, handleSelect];
}