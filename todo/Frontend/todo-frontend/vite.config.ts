import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig({
  plugins: [react()],
  define: {
    // Заменяем process.env на пустой объект, чтобы избежать ошибок в браузере
    "process.env": {},
    "process": "({ env: {} })",
  },
});