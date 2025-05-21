describe("UiTests", () => {
    const URL = "http://localhost:5142/api/App";
    const SELECTORS = {
        title: "#title",
        newPost: "#newPost",
        newPostBtn: "#newPostSubmit",
        errorMess: ".error-message",
        priority: "#priority",
        deadline: "#deadline",
        description: "#description",
        status: "#status",
        updatePost: "#updatdeEdit",
        redactTitle: "#redact-title",
        editDeadline: "#redact-time",
        priorityFilter: "#priority-filter",
        addSort: "#submit-sort",
        taskContainer: ".task-block",
        sortFilter: "#sort",
    };

    const ERROR_MESSAGE = {
        shortTitle: "Длина названия минимум 4 символа",
        longTitle: "Максимальная длина названия максимум 255 символа",
    };

    const badTitles = [
        { title: "123", testName: "Некорректная длина названия(3 символа)", errorMessage: ERROR_MESSAGE.shortTitle },
        { title: "1".repeat(256), testName: "Некорректная длина названия(256 символов)", errorMessage: ERROR_MESSAGE.longTitle },
    ];

    const createTesterTask = () => {
        const cardsDatas = [
            { title: "1234!before 10.10.2025", description: "test description1", deadline: null, priority: "HIGH"},
            { title: "5678", description: "test description2", deadline: "2025-05-22T10:10", priority: "CRITICAL"},
            { title: "9101!3", description: "test description3", deadline: "2025-05-20T10:10", priority: null},
        ];

        cardsDatas.forEach((card) => {
            cy.get(SELECTORS.newPost).click();
            cy.get(SELECTORS.title).clear().type(card.title);
            if (card.description !== null){
                cy.get(SELECTORS.description).clear().type(card.description);
            }
            if (card.deadline !== null){
                cy.get(SELECTORS.deadline).clear().type(card.deadline);
            }
            if (card.priority !== null){
                cy.get('.modal.active').find(SELECTORS.priority).select(card.priority);
            }
            cy.get(SELECTORS.newPostBtn).click();
        })
    }

    beforeEach(() => cy.visit("/"));
    
    context("Валидация формы", () => {

        badTitles.forEach((badTest) => {
            it(badTest.testName, () => {
                cy.get(SELECTORS.newPost).click();
                cy.get(SELECTORS.title).clear().type(badTest.title);
                cy.get(SELECTORS.newPostBtn).click();
                cy.get(SELECTORS.errorMess).should("be.visible").and("contain", badTest.errorMessage);
            })
        });
    });

    context("Создание задач", () => {
        beforeEach(() => {
            cy.request("DELETE", `${URL}/clear`).then(() => {
                cy.get(SELECTORS.newPost).click();  
            });
        });

        it("Проверка задачи с заголовком в 4 символа", () => {
            const title = "1234";
            cy.get(SELECTORS.title).clear().type(title);
            cy.get(SELECTORS.newPostBtn).click();
            cy.contains(title).should('exist');
            cy.request("GET", `${URL}/tasks`).then((result) => {
                const tasks = result.body;
                expect(tasks[0].title).to.equal(title);
            });
        });

        it("Првоеряем установку приоритета макросом", () => {
            const title = "bimbimbamb!3";
            const clearTitle = "bimbimbamb";
            cy.get(SELECTORS.title).clear().type(title);
            cy.get(SELECTORS.newPostBtn).click();
            cy.request("GET", `${URL}/tasks`).then((result) => {
                const tasks = result.body;
                expect(tasks[0].title).to.equal(clearTitle);
                expect(tasks[0].priority).to.equal("MEDIUM");
            });
        });

        it("Првоеряем установку приоритета макросом, но с выбранным значением в поле", () => {
            const title = "bimbimbamb!3";
            const clearTitle = "bimbimbamb";
            cy.get(SELECTORS.title).clear().type(title);
            cy.get('.modal.active').find(SELECTORS.priority).select('HIGH');
            cy.get(SELECTORS.newPostBtn).click();
            cy.request("GET", `${URL}/tasks`).then((result) => {
                const tasks = result.body;
                expect(tasks[0].title).to.equal(clearTitle);
                expect(tasks[0].priority).to.equal("HIGH");
            });
        });

        it("Првоеряем установку даты макросом", () => {
            const title = "bimbimbamb!before 10.10.2025";
            const clearTitle = "bimbimbamb";
            cy.get(SELECTORS.title).clear().type(title);
            cy.get(SELECTORS.newPostBtn).click();
            cy.request("GET", `${URL}/tasks`).then((result) => {
                const tasks = result.body;
                expect(tasks[0].title).to.equal(clearTitle);
                expect(tasks[0].deadline.startsWith("2025-10-10T07:00")).to.be.true;
            });
        });

        it("Првоеряем установку даты макросом, но с выбранным значением в поле", () => {
            const title = "bimbimbamb!before 10.01.2025";
            const clearTitle = "bimbimbamb";
            cy.get(SELECTORS.title).clear().type(title);
            cy.get(SELECTORS.deadline).clear().type("2025-10-10T10:10");
            cy.get(SELECTORS.newPostBtn).click();
            cy.request("GET", `${URL}/tasks`).then((result) => {
                const tasks = result.body;
                expect(tasks[0].title).to.equal(clearTitle);
                expect(tasks[0].deadline.startsWith("2025-10-10T10:10")).to.be.true;
            });
        });

        it("Првоеряем установку даты и макросом", () => {
            const title = "!1bimbimbamb!before 10.10.2025";
            const clearTitle = "bimbimbamb";
            cy.get(SELECTORS.title).clear().type(title);
            cy.get(SELECTORS.newPostBtn).click();
            cy.request("GET", `${URL}/tasks`).then((result) => {
                const tasks = result.body;
                expect(tasks[0].title).to.equal(clearTitle);
                expect(tasks[0].deadline.startsWith("2025-10-10T07:00")).to.be.true;
                expect(tasks[0].priority).to.equal("CRITICAL");
            });
        });

        it("Првоеряем установку даты и макросом, но с выбранным значением в поле", () => {
            const title = "bimbimbamb!before 10.01.2025 !1";
            const clearTitle = "bimbimbamb ";
            cy.get(SELECTORS.title).clear().type(title);
            cy.get(SELECTORS.deadline).clear().type("2025-10-10T10:10");
            cy.get('.modal.active').find(SELECTORS.priority).select('HIGH');
            cy.get(SELECTORS.newPostBtn).click();
            cy.request("GET", `${URL}/tasks`).then((result) => {
                const tasks = result.body;
                expect(tasks[0].title).to.equal(clearTitle);
                expect(tasks[0].deadline.startsWith("2025-10-10T10:10")).to.be.true;
                expect(tasks[0].priority).to.equal("HIGH");
            });
        });
    });

    context("Проверка цвета карточек и статуса", () => {
        beforeEach(() => {
            cy.request("DELETE", `${URL}/clear`).then(() => {
                createTesterTask();
            });
        });

        it("Проверка цвета и статуса у карточки с активным статусом и дедлайном больше чем через 3 дня", () => {
            cy.contains('.task-card .title', '1234')
                .should('exist')
                .parents('.task-card') 
                .as('targetCard');

            cy.get('@targetCard').should('have.css', 'background-color', 'rgb(244, 242, 242)');

            cy.get('@targetCard').find('.title').click();
            cy.get('.modal.active').should('be.visible');
            cy.get('.modal.active').contains('Активно').should('exist');
        });

        it("Проверка цвета и статуса у карточки с активным статусом и дедлайном меньше чем через 3 дня", () => {
            cy.contains('.task-card .title', '5678')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');

            cy.get('@targetCard').should('have.css', 'background-color', 'rgb(237, 157, 66)');

            cy.get('@targetCard').find('.title').click();
            cy.get('.modal.active').should('be.visible');
            cy.get('.modal.active').contains('Активно').should('exist');
        });

        it("Проверка цвета и статуса у карточки с просроченным статусом", () => {
            cy.contains('.task-card .title', '9101')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');

            cy.get('@targetCard').should('have.css', 'background-color', "rgb(237, 66, 66)");

            cy.get('@targetCard').find('.title').click();
            cy.get('.modal.active').should('be.visible');
            cy.get('.modal.active').contains('Просрочено').should('exist');
        });
    })

    context("Проверка изменения статуса", () => {
        beforeEach(() => {
            cy.request("DELETE", `${URL}/clear`).then(() => {
                createTesterTask();
            });
        });

        it("Перевод в выполенную обычной карточки и проверка ее статуса", () => {
            cy.contains('.task-card .title', '1234')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');

            cy.get("@targetCard").find(".changer-status").click();
            cy.contains('.task-card .title', '1234')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');
            cy.get('@targetCard').find('.title').click();
            cy.get('.modal.active').should('be.visible');
            cy.get('.modal.active').contains('Выполнено').should('exist');
        });

        it("Перевод в выполенную карточку у который делик менее чем через 3 дня и проверка ее статуса", () => {
            cy.contains('.task-card .title', '5678')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');

            cy.get("@targetCard").find(".changer-status").click();
            cy.contains('.task-card .title', '5678')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');
            cy.get('@targetCard').find('.title').click();
            cy.get('.modal.active').should('be.visible');
            cy.get('.modal.active').contains('Выполнено').should('exist');
        });

        it("Перевод в выполенную карточку, которая просрочена", () => {
            cy.contains('.task-card .title', '9101')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');

            cy.get("@targetCard").find(".changer-status").click();
            cy.contains('.task-card .title', '9101')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');
            cy.get('@targetCard').find('.title').click();
            cy.get('.modal.active').should('be.visible');
            cy.get('.modal.active').contains('Выполнено с опзданием').should('exist');
        });

        it("Перевод из выполенного состояние в обычное у карточки и проверка ее статуса", () => {
            cy.contains('.task-card .title', '1234')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');

            cy.get("@targetCard").find(".changer-status").click();
            cy.contains('.task-card .title', '1234')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');
            cy.get("@targetCard").find(".changer-status").click();
            cy.contains('.task-card .title', '1234')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');
            cy.get('@targetCard').find('.title').click();
            cy.get('.modal.active').should('be.visible');
            cy.get('.modal.active').contains('Активно').should('exist');
        });

        it("из выполенного состояние в обычное у карточки у который делик менее чем через 3 дня и проверка ее статуса", () => {
            cy.contains('.task-card .title', '5678')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');

            cy.get("@targetCard").find(".changer-status").click();
            cy.contains('.task-card .title', '5678')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');
            cy.get("@targetCard").find(".changer-status").click();
            cy.contains('.task-card .title', '5678')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');
            cy.get('@targetCard').find('.title').click();
            cy.get('.modal.active').should('be.visible');
            cy.get('.modal.active').contains('Активно').should('exist');
        });

        it("Перевод в выполенную карточку, которая просрочена", () => {
            cy.contains('.task-card .title', '9101')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');

            cy.get("@targetCard").find(".changer-status").click();
            cy.contains('.task-card .title', '9101')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');
            cy.get("@targetCard").find(".changer-status").click();
            cy.contains('.task-card .title', '9101')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');
            cy.get('@targetCard').find('.title').click();
            cy.get('.modal.active').should('be.visible');
            cy.get('.modal.active').contains('Просрочено').should('exist');
        });
    });

    context("Проверка удаления карточки", () => {
        beforeEach(() => {
            cy.request("DELETE", `${URL}/clear`).then(() => {
                createTesterTask();
            });
        });
        it("Удаление карточки", () => {
            cy.contains('.task-card .title', '1234')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');

            cy.get("@targetCard").find(".delete").click();
            cy.contains('.task-card .title', '1234')
                .should('not.exist')
        })
    });

    context("Редактирование карточки", () => {
        beforeEach(() => {
            cy.request("DELETE", `${URL}/clear`).then(() => {
                createTesterTask();
            });
        });

        badTitles.forEach((badTest) => {
            it(badTest.testName, () => {
                cy.contains('.task-card .title', '1234')
                    .should('exist')
                    .parents('.task-card')
                    .as('targetCard');
                cy.get("@targetCard").find(".redact").click();
                cy.get(SELECTORS.redactTitle).clear().type(badTest.title);
                cy.get(SELECTORS.updatePost).click();
                cy.get(SELECTORS.errorMess).should("be.visible").and("contain", badTest.errorMessage);
            })
        });

        it("Редактирование даты у просроченной задачи", () => {
            cy.contains('.task-card .title', '9101')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');

            cy.get("@targetCard").find(".changer-status").click();
            cy.contains('.task-card .title', '9101')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');
            cy.get("@targetCard").find(".redact").click();
            cy.get("@targetCard").find(SELECTORS.editDeadline).clear().type("2025-10-10T10:10");
            cy.get("@targetCard").find(SELECTORS.updatePost).click();
            cy.contains('.task-card .title', '9101')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');
            cy.get('@targetCard').should('have.css', 'background-color', 'rgb(244, 242, 242)');
            cy.get('@targetCard').find('.title').click();
            cy.get('.modal.active').should('be.visible');
            cy.get('.modal.active').contains('Активно').should('exist');
        })

        it("Редактирование даты у просроченной задачи", () => {
            cy.contains('.task-card .title', '9101')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');

            cy.get("@targetCard").find(".changer-status").click();
            cy.contains('.task-card .title', '9101')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');
            cy.get("@targetCard").find(".redact").click();
            cy.get("@targetCard").find(SELECTORS.editDeadline).clear().type("2025-10-10T10:10");
            cy.get("@targetCard").find(SELECTORS.updatePost).click();
            cy.contains('.task-card .title', '9101')
                .should('exist')
                .parents('.task-card')
                .as('targetCard');
            cy.get('@targetCard').should('have.css', 'background-color', 'rgb(244, 242, 242)');
            cy.get('@targetCard').find('.title').click();
            cy.get('.modal.active').should('be.visible');
            cy.get('.modal.active').contains('Активно').should('exist');
        })
    })

    context("Фильтрация задач", () => {
        beforeEach(() => {
            cy.request("DELETE", `${URL}/clear`).then(() => {
                createTesterTask();
            });
        });
        
        it("Фильтруем по приоритету HIGH", () => {
            cy.get(SELECTORS.priorityFilter).select('HIGH');
            cy.get(SELECTORS.addSort).click();
            
            cy.get(SELECTORS.taskContainer).should("have.length", 1);

            cy.contains('.task-card .title', '1234')
                        .should('exist')
                        .parents('.task-card')
                        .as('targetCard');
                    cy.get('@targetCard').find('.title').click();
                    cy.get('.modal.active').should('be.visible');
                    cy.get('.modal.active').contains('Высокий').should('exist');
        })

        it("Фильтруем по приоритету CRITICAL", () => {
            cy.get(SELECTORS.priorityFilter).select('CRITICAL');
            cy.get(SELECTORS.addSort).click();

            cy.get(SELECTORS.taskContainer).should("have.length", 1);

            cy.contains('.task-card .title', '5678')
                        .should('exist')
                        .parents('.task-card')
                        .as('targetCard');
                    cy.get('@targetCard').find('.title').click();
                    cy.get('.modal.active').should('be.visible');
                    cy.get('.modal.active').contains('Критический').should('exist');
        });
        
        it("Сортируем по возрастанию", () => {
            cy.get(SELECTORS.sortFilter).select("CreateAsc");
            cy.get(SELECTORS.addSort).click();
            const expectedTitles = ['1234', '5678', '9101'];
            cy.get('.task-card .title').each(($el, index) => {
                expect($el.text().trim()).to.equal(expectedTitles[index]);
            });
        })

        it("Сортируем по убыванию", () => {
            const expectedTitles = ['9101', '5678', '1234'];

            cy.get(SELECTORS.sortFilter).select("CreateDesc");

            cy.get(SELECTORS.addSort).click();

            cy.get('.task-card .title')
                .should('have.length', expectedTitles.length)
                .then(($titles) => {
                    const actualTitles = [...$titles].map((el) => el.textContent?.trim());
                    expect(actualTitles).to.deep.equal(expectedTitles);
                });
        });

    });

})