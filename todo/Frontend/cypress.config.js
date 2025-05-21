const { defineConfig } = require("cypress");

module.exports = defineConfig({
  e2e: {
    specPattern: "cypress/e2e/**/*.cy.{js,jsx,ts,tsx}",
    supportFile: "cypress/support/e2e.js",
    baseUrl: "http://localhost:5173/",
    setupNodeEvents(on, config) {
    },
  },
});
