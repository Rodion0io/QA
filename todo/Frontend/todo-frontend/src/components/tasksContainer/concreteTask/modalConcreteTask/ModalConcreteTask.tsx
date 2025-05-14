import "./modalConcreteTask.css"

import Modal from "../../../modal/Modal";

import {PRIORITY} from "../../../../utils/constants/priority.ts";
import {STATUS} from "../../../../utils/constants/status.ts";

import { modifyDate } from "../../../../utils/modifyDate";

import type {Task} from "../../../../@types/api";

interface ModalConcreteTaskProps{
    modalActive: boolean,
    setModalActive:(flag: boolean) => void,
    taskInformation: Task
}

const ModalConcreteTask = ({ modalActive, setModalActive, taskInformation }: ModalConcreteTaskProps) => {

    return (
        <>
            <Modal modalActive={modalActive} setModalActive={setModalActive}>
                <section className="modal-infa-block">
                    <div className="modal-infa-block_container">
                        <h2 className="modal-infa-block_title">Задача</h2>
                        <h3 className="task-title">Название: {taskInformation.title}</h3>
                        {taskInformation.description !== null ?
                            <>
                                <h3 className="task-title">Описание:</h3>
                                <p className="description">{taskInformation.description}</p>
                            </>
                            : null
                        }
                        {taskInformation.updatedTime ?
                            <>
                                <p className="deadilne-text">Обновлено: {modifyDate(taskInformation.updatedTime)}</p>
                            </>
                            : null
                        }
                        {
                            taskInformation.deadline ?
                                <>
                                    <p className="deadilne-text">Срок: {modifyDate(taskInformation.deadline)}</p>
                                </>:
                                null
                        }
                        <div className="meta-infa-block">
                            <p className="priority">Приоритет: {PRIORITY[taskInformation.priority]}</p>
                            <p className="status">{STATUS[taskInformation.status]}</p>
                        </div>
                    </div>
                </section>
            </Modal>
        </>
    )
};

export default ModalConcreteTask;