import React from "react";
import "./button.css"

type ButtonType = "button";

interface ButtonProps extends React.ComponentProps<"button">{
    buttonType?: ButtonType,
    text: string
}

const Button = ({ className, buttonType = "button", text, ...props}: ButtonProps) => {

    return (
        <>
            <button className={`btn ${className}`} {...props}>{text}</button>
        </>
    )
};

export default Button;