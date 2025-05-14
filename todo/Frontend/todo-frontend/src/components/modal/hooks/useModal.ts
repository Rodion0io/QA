import { useState } from "react";

export const useModal = (): [boolean, () => void] => {

    const [modalActive, setModalActive] = useState<boolean>(false);

    const handleModalState = () => {
        setModalActive((prev) => prev ? false : true);
    }

    return [modalActive, handleModalState];
}