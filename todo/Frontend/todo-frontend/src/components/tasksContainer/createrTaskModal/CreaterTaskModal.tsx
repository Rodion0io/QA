import "./createrTaskModal.css"

import Modal from "../../modal/Modal"
import Input from "../../ui/input/Input"
import Button from "../../ui/button/Button"
import Select from "../../ui/select/Select"

import { useCreater } from "./hooks/useCreater"

interface CreaterTaskProps{
    modalActive: boolean,
    setModalActive:(flag: boolean) => void
}

const CreaterTaskModal = ({ modalActive, setModalActive }: CreaterTaskProps) => {

    const [handleChange, handleClick, errorFlag, errorMessage] = useCreater();

    return (
        <>
            <Modal modalActive={modalActive} setModalActive={setModalActive}>
                <section className="modal-infa-block">
                    <div className="modal-infa-block_container">
                        <h2 className="modal-infa-block_title">Новая задача</h2>
                        <p className="infa">Вы можете установить приоритет или дату через макрос в названии, поля со звездочкой обязательны</p>
                        <div className="input-block">
                            <p className="subtitle">Название*</p>
                            <Input
                                id="title"
                                inputType="input"
                                className="creater-input"
                                handleChanger={(value) => handleChange('title', value)}
                            />

                        </div>
                        <div className="input-block">
                            <p className="subtitle">Описание</p>
                            <Input
                                id="description"
                                inputType="input"
                                className="creater-input"
                                handleChanger={(value) => handleChange('description', value)}
                            />
                        </div>
                        <div className="input-block">
                            <p className="subtitle">Дедлайн</p>
                            <Input
                                id="deadline"
                                inputType="datetime-local"
                                className="creater-input"
                                handleChanger={(value) => handleChange('deadline', value)}
                            />
                        </div>
                        <div className="input-block">
                            <p className="subtitle">Приоритет</p>
                            <Select
                                id="priority"
                                className="creater"
                                valuesArrPriority={["", "LOW" , "MEDIUM" , "HIGH" , "CRITICAL"]}
                                name="priori"
                                selectChange={(value) => handleChange('priority', value)}
                            />
                        </div>
                        <div className="modal-actions-block">
                            <Button text="Создать" id="newPostSubmit" className="submit" onClick={handleClick}/>
                        </div>
                        {errorFlag ?
                            <p className="error-message">{errorMessage}</p>:
                            null
                        }
                    </div>
                </section>
            </Modal>
        </>
    )
};

export default CreaterTaskModal;