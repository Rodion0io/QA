using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using todo.Datas;
using todo.DTO;
using todo.enums;
using Xunit;

namespace todo.test.api_test;

public class ApiTest : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ApiTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    public async Task InitializeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private StringContent JsonSerialize(object model)
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        return new StringContent(
            JsonSerializer.Serialize(model, options),
            Encoding.UTF8,
            "application/json"
        );
    }

    private async Task<List<TaskPreviewModel>> GetListTasks(string Params = "")
    {
        JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };
        var response = await _client.GetAsync($"/api/App/tasks{Params}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<TaskPreviewModel>>(body, jsonOptions)!;
    }

    /// <summary>
    /// Создания карточки
    /// </summary>
    [Fact]
    public async Task CreateTask_WithValidData_ReturnsOk()
    {
        var model = new CreateTaskRequestDto("1233wahjerbfskuwejhb", null, null, Priority.LOW);
        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);

        var task = tasks.Single();
        task.title.Should().Be(model.title);
        task.description.Should().Be(model.description);
        task.priority.Should().Be(model.priority);
        task.deadline.Should().Be(null);
    }

    /// <summary>
    /// Создание задачи с макросом приоритета
    /// </summary>
    [Fact]
    public async Task CreateTask_WithMacrossPriorityValidData_ReturnsOk()
    {
        var model = new CreateTaskRequestDto(
            "1233 !3",
            "bimbimbambammmmm",
            DateTime.SpecifyKind(DateTimeOffset.Parse("2025-12-31T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc),
            null);

        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);

        var task = tasks.Single();
        task.title.Should().Be("1233 ");
        task.description.Should().Be(model.description);
        task.priority.Should().Be(Priority.MEDIUM);
        task.deadline.Should().Be(
            DateTime.SpecifyKind(DateTimeOffset.Parse("2025-12-31T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc).AddHours(7));
    }

    /// <summary>
    /// создание задачи с макросом даты
    /// </summary>
    [Fact]
    public async Task CreateTask_WithMacrossDateValidData_ReturnsOk()
    {
        var model = new CreateTaskRequestDto(
            "1233 !before 10.10.2025",
            "bimbimbambammmmm",
            null,
            Priority.CRITICAL);

        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);

        var task = tasks.Single();
        task.title.Should().Be("1233 ");
        task.description.Should().Be(model.description);
        task.priority.Should().Be(model.priority);
        task.deadline.Should().Be(DateTime.SpecifyKind(DateTimeOffset.Parse("2025-10-10T00:00:00Z").UtcDateTime.AddHours(7), DateTimeKind.Utc));
    }
    
    /// <summary>
    /// Проверяем на граничные значения(4 символа в названии)
    /// </summary>
    [Fact]
    public async Task CreateTask_ValidDataWithMinLengthTitle_ReturnsOk()
    {
        var model = new CreateTaskRequestDto(
            "1233",
            "bimbimbambammmmm",
            null,
            Priority.CRITICAL);

        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);

        var task = tasks.Single();
        task.title.Should().Be(model.title);
        task.description.Should().Be(model.description);
        task.priority.Should().Be(model.priority);
        task.deadline.Should().Be(null);
    }
    
    /// <summary>
    /// Проверяем на граничные значения(255 символа в названии)
    /// </summary>
    [Fact]
    public async Task CreateTask_ValidDataWithMaxLengthTitle_ReturnsOk()
    {
        var model = new CreateTaskRequestDto(
            new string('1', 255),
            "bimbimbambammmmm",
            null,
            Priority.CRITICAL);

        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);

        var task = tasks.Single();
        task.title.Should().Be(model.title);
        task.description.Should().Be(model.description);
        task.priority.Should().Be(model.priority);
        task.deadline.Should().Be(null);
    }
    
    /// <summary>
    /// Создание задачи с макросом приоритета и заполеннным полем приоритета
    /// </summary>
    [Fact]
    public async Task CreateTask_ValidDataWithMacrosPriorityAndValue_ReturnsOk()
    {
        var model = new CreateTaskRequestDto(
            "dkhrgbkle !1",
            "bimbimbambammmmm",
            null,
            Priority.CRITICAL);

        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);

        var task = tasks.Single();
        task.title.Should().Be("dkhrgbkle ");
        task.description.Should().Be(model.description);
        task.priority.Should().Be(model.priority);
        task.deadline.Should().Be(null);
    }
    
    /// <summary>
    /// Создание задачи с макросом приоритета и заполеннным полем приоритета
    /// </summary>
    [Fact]
    public async Task CreateTask_ValidDataWithMacrosDateAndValue_ReturnsOk()
    {
        var model = new CreateTaskRequestDto(
            "dkhrgbkle !before 10-10-2025",
            "bimbimbambammmmm",
            DateTime.SpecifyKind(DateTimeOffset.Parse("2025-12-31T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc),
            Priority.CRITICAL);

        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);

        var task = tasks.Single();
        task.title.Should().Be("dkhrgbkle ");
        task.description.Should().Be(model.description);
        task.priority.Should().Be(model.priority);
        task.deadline.Should().Be(DateTime.SpecifyKind(DateTimeOffset.Parse("2025-12-31T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc).AddHours(7));
    }
    
    
    /// <summary>
    /// Создание задачи с длиной названия в 3 символа
    /// </summary>
    [Fact]
    public async Task CreateTask_InValidDataShortTitle_ReturnsBadRequest()
    {
        var model = new CreateTaskRequestDto(
            "123",
            "bimbimbambammmmm",
            DateTime.SpecifyKind(DateTimeOffset.Parse("2025-12-31T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc),
            Priority.CRITICAL);

        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("The field title must be a string or array type with a minimum length of '4'.");

        var tasks = await GetListTasks();
        tasks.Should().BeEmpty();
    }
    
    /// <summary>
    /// Создание задачи с длиной названия в 256 символа
    /// </summary>
    [Fact]
    public async Task CreateTask_InValidDataLongTitle_ReturnsBadRequest()
    {
        var model = new CreateTaskRequestDto(
            new string('1', 256),
            "bimbimbambammmmm",
            DateTime.SpecifyKind(DateTimeOffset.Parse("2025-12-31T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc),
            Priority.CRITICAL);

        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("Название либо больше 3, либо меньше 255");

        var tasks = await GetListTasks();
        tasks.Should().BeEmpty();
    }
    
    /// <summary>
    /// Создание задачи с отсутствием названия карточки
    /// </summary>
    [Fact]
    public async Task CreateTask_InValidDataWithoutTitle_ReturnsBadRequest()
    {
        var model = new CreateTaskRequestDto(
            null,
            "bimbimbambammmmm",
            DateTime.SpecifyKind(DateTimeOffset.Parse("2025-12-31T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc),
            Priority.CRITICAL);

        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("One or more validation errors occurred.");

        var tasks = await GetListTasks();
        tasks.Should().BeEmpty();
    }
    
    /// <summary>
    /// прошлая дата
    /// </summary>
    [Fact]
    public async Task CreateTask_InValidDataPastDate_ReturnsBadRequest()
    {
        var model = new CreateTaskRequestDto(
            "rhisbdbfnks",
            "bimbimbambammmmm",
            DateTime.SpecifyKind(DateTimeOffset.Parse("2012-12-31T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc),
            null);

        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadAsStringAsync();
        error.Should().Contain("Указанное время не может бюыть меньше или равно текущему!");

        var tasks = await GetListTasks();
        tasks.Should().BeEmpty();
    }
    
    /// <summary>
    /// Получение списка задач
    /// </summary>
    [Fact]
    public async Task GetListTask_WithValidData_ReturnsOk()
    {
        var firstModel = new CreateTaskRequestDto("1233wahjerbfskuwejhb", null, null, Priority.LOW);
        var firstContent = JsonSerialize(firstModel);
        var firstResponse = await _client.PostAsync("/api/App/create", firstContent);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var secondModel = new CreateTaskRequestDto("1234", null, null, Priority.LOW);
        var secondContent = JsonSerialize(secondModel);
        var secondResponse = await _client.PostAsync("/api/App/create", secondContent);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var list = await GetListTasks();
        list.Should().HaveCount(2);
        
        list[0].title.Should().Be("1233wahjerbfskuwejhb");
        list[1].title.Should().Be("1234");
    }
    
    /// <summary>
    /// Получение списка задач c сортировкой по возрастанию и убыванию
    /// </summary>
    [Fact]
    public async Task GetListTaskWithSortCreater_WithValidData_ReturnsOk()
    {
        var firstModel = new CreateTaskRequestDto("1234", null, null, Priority.LOW);
        var firstContent = JsonSerialize(firstModel);
        var firstResponse = await _client.PostAsync("/api/App/create", firstContent);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var secondModel = new CreateTaskRequestDto("12345", null, null, Priority.LOW);
        var secondContent = JsonSerialize(secondModel);
        var secondResponse = await _client.PostAsync("/api/App/create", secondContent);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var listAsc = await GetListTasks("?sort=CreateAsc");
        listAsc.Should().HaveCount(2);
        
        listAsc[0].title.Should().Be("1234");
        listAsc[1].title.Should().Be("12345");
        
        var listDesc = await GetListTasks("?sort=CreateDesc");
        listDesc.Should().HaveCount(2);
        
        listDesc[0].title.Should().Be("12345");
        listDesc[1].title.Should().Be("1234");
    }
    
    /// <summary>
    /// Получение списка задач с сортировкой и выборкой задач конкретного проиритета
    /// </summary>
    [Fact]
    public async Task GetListTaskWithSortCreaterAndPriority_WithValidData_ReturnsOk()
    {
        var firstModel = new CreateTaskRequestDto("1234", null, null, Priority.LOW);
        var firstContent = JsonSerialize(firstModel);
        var firstResponse = await _client.PostAsync("/api/App/create", firstContent);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var secondModel = new CreateTaskRequestDto("12345", null, null, Priority.MEDIUM);
        var secondContent = JsonSerialize(secondModel);
        var secondResponse = await _client.PostAsync("/api/App/create", secondContent);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var thirdModel = new CreateTaskRequestDto("123456", null, null, Priority.MEDIUM);
        var thirdContent = JsonSerialize(thirdModel);
        var thridResponse = await _client.PostAsync("/api/App/create", thirdContent);
        thridResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var listAsc = await GetListTasks("?priority=MEDIUM&sort=CreateAsc");
        listAsc.Should().HaveCount(2);
        
        listAsc[0].title.Should().Be("12345");
        listAsc[1].title.Should().Be("123456");
        
        var listDesc = await GetListTasks("?priority=MEDIUM&sort=CreateDesc");
        listDesc.Should().HaveCount(2);
        
        listDesc[0].title.Should().Be("123456");
        listDesc[1].title.Should().Be("12345");
        
        var withoutPriority = await GetListTasks("?priority=CRITICAL");
        withoutPriority.Should().HaveCount(0);
    }
    
    /// <summary>
    /// Получение списка задач с сортировкой не валидный урл
    /// </summary>
    [Fact]
    public async Task GetListTaskWithSortCreaterAndPriority_WithInvalidUrl_ReturnsBadRequest()
    {
        var firstModel = new CreateTaskRequestDto("1234", null, null, Priority.LOW);
        var firstContent = JsonSerialize(firstModel);
        var firstResponse = await _client.PostAsync("/api/App/create", firstContent);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var secondModel = new CreateTaskRequestDto("12345", null, null, Priority.MEDIUM);
        var secondContent = JsonSerialize(secondModel);
        var secondResponse = await _client.PostAsync("/api/App/create", secondContent);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    
        var listResponse = await _client.GetAsync("/api/App/tasks?sort=Creasdgsdg");

        listResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await listResponse.Content.ReadAsStringAsync();

        error.Should().Contain("Такого вида сортировки нет");
    }

    /// <summary>
    /// Получение конкретного задания
    /// </summary>
    [Fact]
    public async Task GetConcreteTask_WithValidData_ReturnsOk()
    {
        var firstModel = new CreateTaskRequestDto("1234", null, null, Priority.LOW);
        var firstContent = JsonSerialize(firstModel);
        var firstResponse = await _client.PostAsync("/api/App/create", firstContent);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var secondModel = new CreateTaskRequestDto("12345", null, null, Priority.MEDIUM);
        var secondContent = JsonSerialize(secondModel);
        var secondResponse = await _client.PostAsync("/api/App/create", secondContent);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    
        var listDesc = await GetListTasks();
    
        var id = listDesc[0].id;

        var response = await _client.GetAsync($"api/App/getTask/{id}");
    
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    
        var body = await response.Content.ReadAsStringAsync();
    
        var task = JsonSerializer.Deserialize<TaskPreviewModel>(body);
    
        task.Should().NotBeNull();
        task.id.Should().Be(id);
    }

    /// <summary>
    /// Получение конкретного задания(не существующий id)
    /// </summary>
    [Fact]
    public async Task GetConcreteTask_WithInValidData_ReturnsBadRequest()
    {
        var firstModel = new CreateTaskRequestDto("1234", null, null, Priority.LOW);
        var firstContent = JsonSerialize(firstModel);
        var firstResponse = await _client.PostAsync("/api/App/create", firstContent);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var secondModel = new CreateTaskRequestDto("12345", null, null, Priority.MEDIUM);
        var secondContent = JsonSerialize(secondModel);
        var secondResponse = await _client.PostAsync("/api/App/create", secondContent);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    
        var listDesc = await GetListTasks();
    
        // var id = listDesc[0].id;

        Guid id = new Guid();

        var response = await _client.GetAsync($"api/App/getTask/{id}");
    
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    /// <summary>
    /// Удаление задачи
    /// </summary>
    [Fact]
    public async Task DeleteTask_WithValidData_ReturnsOk()
    {
        var firstModel = new CreateTaskRequestDto("1234", null, null, Priority.LOW);
        var firstContent = JsonSerialize(firstModel);
        var firstResponse = await _client.PostAsync("/api/App/create", firstContent);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var listDesc = await GetListTasks();
    
        var id = listDesc[0].id;

        var response = await _client.DeleteAsync($"api/App/deleteTask/{id}");
    
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var tasks = await GetListTasks();
        tasks.Should().BeEmpty();
        
    }

    /// <summary>
    /// Удаление задачи(не существующий id)
    /// </summary>
    [Fact]
    public async Task DeleteTask_WithInValidData_ReturnsBadRequest()
    {
        var firstModel = new CreateTaskRequestDto("1234", null, null, Priority.LOW);
        var firstContent = JsonSerialize(firstModel);
        var firstResponse = await _client.PostAsync("/api/App/create", firstContent);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    
        // var listDesc = await GetListTasks();
    
        // var id = listDesc[0].id;

        var InvalidId = new Guid();
    
        var response = await _client.DeleteAsync($"api/App/deleteTask/{InvalidId}");
    
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    
        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);
    }
    
    /// <summary>
    /// Редакттрованрие карточки
    /// </summary>
    [Fact]
    public async Task UpdateTask_WithValidData_ReturnsOk()
    {
        var model = new CreateTaskRequestDto("1233wahjerbfskuwejhb", "1234567890123456789", null, Priority.LOW);
        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);
    
        var editedTaskId = tasks[0].id;

        var editModel = new UpdateTaskDto
        {
            title = "1234",
            description = "kshrbtkwjhebifewbkewbfhk",
            deadline = DateTime.SpecifyKind(DateTime.Parse("2025-05-16T00:00:58.474Z"), DateTimeKind.Utc),
            priority = Priority.MEDIUM
        };
    
        var editContent = JsonSerialize(editModel);
        var editResponse = await _client.PutAsync($"/api/App/update/{editedTaskId}", editContent);
        editResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    
        var afterEditTasks = await GetListTasks();
        afterEditTasks.Should().HaveCount(1);

        var task = afterEditTasks.Single();
        task.title.Should().Be(editModel.title);
        task.description.Should().Be(editModel.description);
        task.priority.Should().Be(editModel.priority);
    }

    /// <summary>
    /// Редакттрованрие карточки(проверка длины 4)
    /// </summary>
    [Fact]
    public async Task UpdateTask_WithValidDatacheckLengthData_ReturnsOk()
    {
        var model = new CreateTaskRequestDto("1233wahjerbfskuwejhb", "1234567890123456789", null, Priority.LOW);
        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);
    
        var editedTaskId = tasks[0].id;

        var editModel = new UpdateTaskDto
        {
            title = "1234",
            description = "kshrbtkwjhebifewbkewbfhk",
            deadline = DateTime.SpecifyKind(DateTime.Parse("2025-05-16T00:00:58.474Z"), DateTimeKind.Utc),
            priority = Priority.LOW
        };
    
        var editContent = JsonSerialize(editModel);
        var editResponse = await _client.PutAsync($"/api/App/update/{editedTaskId}", editContent);
        editResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    
        var afterEditTasks = await GetListTasks();
        afterEditTasks.Should().HaveCount(1);

        var task = afterEditTasks.Single();
        task.title.Should().Be(editModel.title);
        task.description.Should().Be(editModel.description);
        task.priority.Should().Be(editModel.priority);
    }
    
    /// <summary>
    /// Редакттрованрие карточки(проверка длины 255)
    /// </summary>
    [Fact]
    public async Task UpdateTask_WithValidDatacheckLengthDataMax_ReturnsOk()
    {
        var model = new CreateTaskRequestDto("1233wahjerbfskuwejhb", "1234567890123456789", null, Priority.LOW);
        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);
    
        var editedTaskId = tasks[0].id;

        var editModel = new UpdateTaskDto
        {
            title = new string('1',255),
            description = "kshrbtkwjhebifewbkewbfhk",
            deadline = DateTime.SpecifyKind(DateTime.Parse("2025-05-16T00:00:58.474Z"), DateTimeKind.Utc),
            priority = Priority.LOW
        };
    
        var editContent = JsonSerialize(editModel);
        var editResponse = await _client.PutAsync($"/api/App/update/{editedTaskId}", editContent);
        editResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    
        var afterEditTasks = await GetListTasks();
        afterEditTasks.Should().HaveCount(1);

        var task = afterEditTasks.Single();
        task.title.Should().Be(editModel.title);
        task.description.Should().Be(editModel.description);
        task.priority.Should().Be(editModel.priority);
    }
    
    /// <summary>
    /// Редакттрованрие карточки(изменение приоритет)
    /// </summary>
    [Fact]
    public async Task UpdateTask_WithValidDataChangePriority_ReturnsOk()
    {
        var model = new CreateTaskRequestDto("1233wahjerbfskuwejhb", "1234567890123456789", null, Priority.LOW);
        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);
    
        var editedTaskId = tasks[0].id;

        var editModel = new UpdateTaskDto
        {
            title = "sjdfjka",
            description = "kshrbtkwjhebifewbkewbfhk",
            deadline = null,
            priority = Priority.CRITICAL
        };
    
        var editContent = JsonSerialize(editModel);
        var editResponse = await _client.PutAsync($"/api/App/update/{editedTaskId}", editContent);
        editResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    
        var afterEditTasks = await GetListTasks();
        afterEditTasks.Should().HaveCount(1);

        var task = afterEditTasks.Single();
        task.title.Should().Be(editModel.title);
        task.description.Should().Be(editModel.description);
        task.priority.Should().Be(editModel.priority);
    }
    
    /// <summary>
    /// Редакттрованрие карточки не валидный(меньше 4 символов название)
    /// </summary>
    [Fact]
    public async Task UpdateTask_WithInValidDataThreSymbols_ReturnsBadRequest()
    {
        var model = new CreateTaskRequestDto("1233wahjerbfskuwejhb", "1234567890123456789", null, Priority.LOW);
        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);
    
        var editedTaskId = tasks[0].id;

        var editModel = new UpdateTaskDto
        {
            title = "sj",
            description = "kshrbtkwjhebifewbkewbfhk",
            deadline = null,
            priority = Priority.CRITICAL
        };
    
        var editContent = JsonSerialize(editModel);
        var editResponse = await _client.PutAsync($"/api/App/update/{editedTaskId}", editContent);
        editResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    
        var afterEditTasks = await GetListTasks();
        afterEditTasks.Should().HaveCount(1);

        var task = tasks.Single();
        task.title.Should().Be(model.title);
        task.description.Should().Be(model.description);
        task.priority.Should().Be(model.priority);
        task.deadline.Should().Be(model.deadline);
    }
    
    /// <summary>
    /// Редакттрованрие карточки не валидный(больше 255 символов название)
    /// </summary>
    [Fact]
    public async Task UpdateTask_WithInValidDataManySymbols_ReturnsBadRequest()
    {
        var model = new CreateTaskRequestDto("1233wahjerbfskuwejhb", "1234567890123456789", null, Priority.LOW);
        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);
    
        var editedTaskId = tasks[0].id;

        var editModel = new UpdateTaskDto
        {
            title = new string('1', 256),
            description = "kshrbtkwjhebifewbkewbfhk",
            deadline = null,
            priority = Priority.CRITICAL
        };
    
        var editContent = JsonSerialize(editModel);
        var editResponse = await _client.PutAsync($"/api/App/update/{editedTaskId}", editContent);
        editResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    
        var afterEditTasks = await GetListTasks();
        afterEditTasks.Should().HaveCount(1);

        var task = tasks.Single();
        task.title.Should().Be(model.title);
        task.description.Should().Be(model.description);
        task.priority.Should().Be(model.priority);
        task.deadline.Should().Be(model.deadline);
    }
    
    
    /// <summary>
    /// Редакттрованрие карточки не валидный id
    /// </summary>
    [Fact]
    public async Task UpdateTask_WithInValidDataInvalidId_ReturnsBadRequest()
    {
        var model = new CreateTaskRequestDto("1233wahjerbfskuwejhb", "1234567890123456789", null, Priority.LOW);
        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);
    
        var editedTaskId = tasks[0].id;

        var editModel = new UpdateTaskDto
        {
            title = "djsbgkjhsbfjk",
            description = "kshrbtkwjhebifewbkewbfhk",
            deadline = null,
            priority = Priority.CRITICAL
        };

        var invalidId = new Guid();
    
        var editContent = JsonSerialize(editModel);
        var editResponse = await _client.PutAsync($"/api/App/update/{invalidId}", editContent);
        editResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    
        var afterEditTasks = await GetListTasks();
        afterEditTasks.Should().HaveCount(1);

        var task = tasks.Single();
        task.title.Should().Be(model.title);
        task.description.Should().Be(model.description);
        task.priority.Should().Be(model.priority);
        task.deadline.Should().Be(model.deadline);
    }
    
    /// <summary>
    /// Изменение статуса
    /// </summary>
    [Fact]
    public async Task UpdateStatusTask_WithValidData_ReturnsOk()
    {
        var model = new CreateTaskRequestDto("1233wahjerbfskuwejhb", "1234567890123456789", DateTime.SpecifyKind(DateTimeOffset.Parse("2025-12-31T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc), Priority.LOW);
        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);
        
        var id = tasks[0].id;

        var editModel = new UpdateStatusDto
        {
            Status = Status.COMPLETED
        };
    
        var editContent = JsonSerialize(editModel);
        var editResponse = await _client.PutAsync($"/api/App/updateStatus/{id}", editContent);
        editResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    
        var afterEditTasks = await GetListTasks();
        afterEditTasks.Should().HaveCount(1);

        var task = afterEditTasks.Single();
        task.status.Should().Be(editModel.Status);
    }
    
    
    /// <summary>
    /// Изменение статуса(Несуществующая карточка)
    /// </summary>
    [Fact]
    public async Task UpdateStatusTask_WithValidDataWithInvalidId_ReturnsBadRequest()
    {
        var model = new CreateTaskRequestDto("1233wahjerbfskuwejhb", "1234567890123456789", DateTime.SpecifyKind(DateTimeOffset.Parse("2025-12-31T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc), Priority.LOW);
        var content = JsonSerialize(model);
        var response = await _client.PostAsync("/api/App/create", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await GetListTasks();
        tasks.Should().HaveCount(1);
        
        var id = tasks[0].id;

        var invalidId = new Guid();

        var editModel = new UpdateStatusDto
        {
            Status = Status.OVERDUE
        };
    
        var editContent = JsonSerialize(editModel);
        var editResponse = await _client.PutAsync($"/api/App/updateStatus/{invalidId}", editContent);
        editResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    
        var afterEditTasks = await GetListTasks();
        afterEditTasks.Should().HaveCount(1);

        var task = afterEditTasks.Single();
        task.status.Should().Be(Status.ACTIVE);
    }
}
