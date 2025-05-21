import "./filter.css"

import { useFilter } from "./hooks/useFilter";

import Button from "../ui/button/Button";
import Select from "../ui/select/Select";

interface FilterProps{
    handleFilter?:(value: string) => void
}

const Filter = ({ handleFilter }: FilterProps) => {

    const [handleChange, pattern] = useFilter();

    const handleClick = () => {
        if (handleFilter){
            handleFilter(pattern.current);
        }
    }

    return (
        <>
            <section className="filter-block">
                <h2 className="block-title">Фильтры</h2>
                <div className="filters-set">
                    <div className="input-block">
                        <p className="subtitle">Приоритет</p>
                        <Select
                            id="priority-filter"
                            className="creater"
                            
                            valuesArrPriority={["", "LOW" , "MEDIUM" , "HIGH" , "CRITICAL"]}
                            name="priori"
                            selectChange={(value) => handleChange('priority', value)}
                        />
                    </div>
                    <div className="input-block">
                        <p className="subtitle">Сортировать по</p>
                        <Select
                            id="sort"
                            className="creater"
                            valuesArrSort={["", "CreateAsc" , "CreateDesc" , "PriorityAsc" , "PriorityDesc", "StatusAsc", "StatusDesc"]}
                            name="sortBy"
                            selectChange={(value) => handleChange('sort', value)}
                        />
                    </div>
                </div>
                <div className="action-block">
                    <Button id="submit-sort" text="Применить" className="submit filter-btn" onClick={handleClick}/>
                </div>
            </section>
        </>
    )
};

export default Filter;