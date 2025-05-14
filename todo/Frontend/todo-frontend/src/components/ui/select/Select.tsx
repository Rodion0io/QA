import type {SelectionPrioroty, SelectionSort} from "../../../@types/api";

import {PRIORITY} from "../../../utils/constants/priority.ts";
import {SORT} from "../../../utils/constants/sort.ts";

import { useSelect } from "./hooks/useSelect";

import "./select.css"

interface SelectProps extends React.ComponentProps<'select'>{
    valuesArrPriority?: SelectionPrioroty[],
    valuesArrSort?: SelectionSort[]
    name: string,
    initialValue?: string,
    selectChange?: (value: string) => void
}

const Select = ({ initialValue, className, id, valuesArrPriority, valuesArrSort, name, selectChange, ...props}: SelectProps) => {

    const [selected, handleSelect] = useSelect(initialValue ? initialValue : "", selectChange);

    return (
        <>
            <select
                className={`select ${className}`}
                name={name}
                value={selected}
                id={id}
                onChange={handleSelect}
                {...props}
            >
                {valuesArrPriority ? valuesArrPriority.map((item, index) => (
                    <option key={index} value={item}>{PRIORITY[item]}</option>
                )): valuesArrSort ?
                    valuesArrSort.map((item, index) => (
                        <option key={index} value={item}>{SORT[item]}</option>
                    )):
                    undefined
                }
            </select>
        </>
    )
};

export default Select;