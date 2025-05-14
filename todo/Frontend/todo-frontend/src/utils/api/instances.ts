import axios from "axios"

import { URL } from "../constants/constants";

export const RequestPattern = axios.create({
    baseURL: URL,
    headers: {
        "Content-Type": "application/json",

    }
});