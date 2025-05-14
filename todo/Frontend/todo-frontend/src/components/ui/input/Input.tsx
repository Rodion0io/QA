import "./input.css"

import { useInput } from "./hooks/useInput";

type InputType = "input" | "datetime-local";

interface InputProps extends React.ComponentProps<'input'>{
    inputType: InputType,
    initialValue?: string,
    handleChanger?: (value: string) => void,
}

const Input = ({ inputType = "input", className, initialValue, handleChanger, ...props}: InputProps) => {

    const [inputValue, handleChange] = useInput(initialValue ? initialValue : "", handleChanger);

    return (
        <>
            <input
                className={`input ${className}`}
                type={inputType}
                {...props}
                value={inputValue}
                onChange={handleChange}
            />
        </>
    )
};

export default Input;