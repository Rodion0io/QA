import "./card.css"

import type {Task} from "../../@types/api";

import ModalConcreteTask from "../tasksContainer/concreteTask/modalConcreteTask/ModalConcreteTask"
import RedactModal from "../redactModal/RedactModal"

import { modifyDate } from "../../utils/modifyDate"
import { checkStatus } from "../../utils/checkStatus"
import { checkStatusTask } from "../../utils/checkStatusTask"

import redactIcon from "../../../public/pencil.svg"
import deleteIcon from "../../../public/trash.svg"
import checkbox from "../../../public/icons8-пустой-чекбокс-100.png"
import selectedCheckbox from "../../../public/icons8-отмеченный-чекбокс-100.png"

import { handleDelete } from "./handles/handleDelete"
import { handleChangeStatus } from "./handles/handleChangeStatus"
import { useModal } from "../modal/hooks/useModal"


interface CardProps{
    props: Task
}

const Card = ({ props }: CardProps) => {

    const [modalConcreteActive, handleModalConcreteState] = useModal();
    const [modalUpdateActive, handleModalUpdateState] = useModal();

    return (
        <>
            <article className={`${props.deadline ? checkStatusTask(props.deadline, props.status) : null} task-card`}>
                <img
                    src={checkStatus(props.status)
                        ? checkbox
                        : selectedCheckbox}
                    alt={checkStatus(props.status) ? "ACTIVE" : "COMPLETED"} className="action-button"
                    id={props.id}
                    onClick={handleChangeStatus}
                />
                <div className="infromation">
                    <h2 className="title" onClick={handleModalConcreteState}>{props.title}</h2>
                    <ModalConcreteTask
                        modalActive={modalConcreteActive}
                        setModalActive={handleModalConcreteState}
                        taskInformation={props}
                    />
                    {props.deadline ?
                        <span className="deadline">{modifyDate(props.deadline)}</span>:
                        null
                    }
                </div>
                {/* {props.deadline ? checkStatusTask(props.deadline, props.status) === "completed" ||
                    checkStatusTask(props.deadline, props.status) === "late" ?
                    undefined :
                    <img src={redactIcon} alt="" className="action-button" id={props.id} onClick={handleModalUpdateState}/>:
                    null
                } */}
                {/* {props.deadline ? */}
                <img src={redactIcon} alt="" className="action-button" id={props.id} onClick={handleModalUpdateState}/>
                {/* null */}
                {/* } */}
                <RedactModal
                    modalActive={modalUpdateActive}
                    setModalActive={handleModalUpdateState}
                    taskInformation={props}
                />
                <img src={deleteIcon} alt="" className="action-button" id={props.id} onClick={handleDelete}/>
            </article>
        </>
    )
};

export default Card;