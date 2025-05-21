import "./tasksContainer.css"
import addIcon from "../../../public/plus.svg"

import Card from "../card/Card";
import CreaterTaskModal from "./createrTaskModal/CreaterTaskModal";
import Filter from "../filter/Filter";

import { useModal } from "../modal/hooks/useModal";
import { useRequest } from "./hooks/useRequest";
import { useState } from "react";

const TasksContainer = () => {

    const [urlPart, setUrlPart] = useState<string>("");
    const tasks = useRequest({urlPattern: urlPart});
    const [modalActive, handleModalState] = useModal();

    const handle = (value: string) => {
        setUrlPart(value);
    }

    return (
        <>
            <section className="task-container">
                <Filter handleFilter={(value) => handle(value)}/>
                <div className="head-block">
                    <h2 className="block-title">Задачи</h2>
                    <img id="newPost" className="action-button"
                         src={addIcon} alt="" onClick={handleModalState}/>
                </div>
                {tasks.length === 0 ?
                    <p>Список пуст</p>:
                    <div className="task-block">
                        {tasks.map((item) => (
                            <Card props={item} key={item.id}/>
                        ))}
                    </div>
                }
                <CreaterTaskModal modalActive={modalActive} setModalActive={handleModalState}/>
            </section>
        </>
    )
};

export default TasksContainer;