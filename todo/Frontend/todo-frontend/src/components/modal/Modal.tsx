import React from "react";

import "./modal.css"

interface ModalProps{
    modalActive: boolean,
    setModalActive:(flag: boolean) => void,
    children: React.ReactNode
}

const Modal = ({ modalActive, setModalActive, children }: ModalProps) => {

    return (
        <>
            <div className={modalActive ? "modal active" : "modal"} onClick={() => setModalActive(false)}>
                <div className="modal-content" onClick={(e) => e.stopPropagation()}>{children}</div>
            </div>
        </>
    )
};

export default Modal;