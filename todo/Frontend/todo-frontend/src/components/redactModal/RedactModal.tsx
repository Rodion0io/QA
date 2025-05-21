import "./redactModal.css"


import type {Task} from "../../@types/api";


import Input from "../ui/input/Input";
import Select from "../ui/select/Select";
import Button from "../ui/button/Button";
import Modal from "../modal/Modal";

import { useUpdate } from "./hooks/useUpdate";


interface ModalConcreteTaskProps {
    modalActive: boolean,
    setModalActive: (flag: boolean) => void,
    taskInformation: Task
}

const RedactModal = ({ modalActive, setModalActive, taskInformation }: ModalConcreteTaskProps) => {
    const [handleChange, handleClick, errorFlag, errorMessage] = useUpdate(taskInformation, taskInformation.id);

    
    //Выдал гпт
    // Функция для преобразования даты в правильный формат для datetime-local
    const formatDateForInput = (dateString: string) => {
        const date = new Date(dateString);
        // Компенсируем смещение временной зоны
        const localDate = new Date(date.getTime() - date.getTimezoneOffset() * 60000);
        return localDate.toISOString().slice(0, 16);
    };

    return (
        <Modal modalActive={modalActive} setModalActive={setModalActive}>
            <section className="modal-infa-block">
                <div className="modal-infa-block_container">
                    <h2 className="modal-infa-block_title">Задача</h2>
                    <div className="input-block">
                        <p className="subtitle">Название*</p>
                        <Input
                            id="redact-title"
                            inputType="input"
                            className="creater-input"
                            initialValue={taskInformation.title}
                            handleChanger={(value) => handleChange('title', value)}
                        />
                    </div>
                    <div className="input-block">
                        <p className="subtitle">Описание</p>
                        <Input
                            id="redact-description"
                            inputType="input"
                            className="creater-input"
                            initialValue={taskInformation.description}
                            handleChanger={(value) => handleChange('description', value)}
                        />
                    </div>
                    <div className="input-block">
                        <p className="subtitle">Дедлайн</p>
                        <Input
                            id="redact-time"
                            inputType="datetime-local"
                            className="creater-input"
                            initialValue={formatDateForInput(taskInformation.deadline)}
                            handleChanger={(value) => handleChange('deadline', value)}
                        />
                    </div>
                    <div className="input-block">
                        <p className="subtitle">Приоритет</p>
                        <Select
                            className="creater"
                            id="priority"
                            initialValue={taskInformation.priority}
                            valuesArrPriority={["", "CRITICAL", "HIGH", "MEDIUM", "LOW"]}
                            name="priori"
                            selectChange={(value) => handleChange('priority', value)}
                        />
                    </div>
                    <div className="modal-actions-block">
                        <Button id="updatdeEdit" text="Обновить" className="submit" onClick={handleClick}/>
                    </div>
                    {errorFlag && <p id="error-redact" className="error-message">{errorMessage}</p>}
                </div>
            </section>
        </Modal>
    )
};

export default RedactModal;