using Microsoft.EntityFrameworkCore;
using todo.constants;
using todo.Datas;
using todo.DTO;
using todo.enums;
using todo.Models;
using todo.Service.Interface;

namespace todo.Service;

public class AppService : IAppService
{
    private readonly DataContext _context;
    private readonly MacrosService _macrosService;

    public AppService(DataContext context, MacrosService macrosService)
    {
        _context = context;
        _macrosService = macrosService;
    }

    public async Task<ResponseModel> NewTask(CreateTaskRequestDto model)
    {
        
        // var newTask = new ToDoModel
        // { 
        //     title = model.title,
        //     description = model.description,
        //     deadline = model.deadline?.ToUniversalTime() ?? DateTime.Today,
        //     priority = model.priority ?? Priority.MEDIUM,
        // };
        
        var newTask = new ToDoModel
        { 
            title = model.title,
            description = model.description,
            // Преобразуем значение времени в UTC, если оно имеет тип Unspecified
            deadline = model.deadline.HasValue 
                ? DateTime.SpecifyKind(model.deadline.Value, DateTimeKind.Utc)
                : null,

            priority = model.priority ?? Priority.MEDIUM,
        };
        
        // Priority.
    
        // if (_macrosService.CheckMacrosDate(model.title, Patterns.dateMacros) == false &&
        //     model.deadline?.ToUniversalTime() == null)
        // {
        //     // return new ResponseModel(400, "Введите дату!");
        // }
        
        if (model.priority == null)
        {
            if (_macrosService.CheckMacrosPriority(model.title, Patterns.priorityMacros))
            {
                Priority priority = _macrosService.GetPriority(model.title, Patterns.priorityMacros);
                
                newTask.priority = priority;
            }
        }
    
        if (model.deadline == null)
        {
            if (_macrosService.CheckMacrosDate(model.title, Patterns.dateMacros))
            {
                DateTime deadline = _macrosService.GetDate(model.title, Patterns.dateMacros);
                
                newTask.deadline = deadline;
            }
            // else
            // {
            //     return new ResponseModel(400, "Установите дедлайн");
            // }
        }
        
        if (newTask.description != null && (newTask.description.Length < 15 || newTask.description.Length > 500))
        {
            return new ResponseModel(400, "Описание либо меньше 15 символов, либо больше 500");
        }
        if (newTask.deadline != null && newTask.deadline?.ToUniversalTime() <= DateTime.UtcNow)
        {
            return new ResponseModel(400, "Указанное время не может бюыть меньше или равно текущему!");
        }

        if (newTask.title.Length < 4 || newTask.title.Length > 255)
        {
            return new ResponseModel(400, "Название либо больше 3, либо меньше 255");
        }
        
        newTask.title = _macrosService.deleteMacros(model.title, Patterns.priorityMacros);
        newTask.title = _macrosService.deleteMacros(newTask.title, Patterns.dateMacros);
        
        await _context.Tasks.AddAsync(newTask);
        await _context.SaveChangesAsync();
        return new ResponseModel(201, "Success"); 
    }
    
    // public async Task<ResponseModel> NewTask(CreateTaskRequestDto model)
    // {
    //     if (_macrosService.CheckMacrosDate(model.title, Patterns.dateMacros) == false &&
    //         model.deadline == null)
    //     {
    //         return new ResponseModel(400, "Введите дату!");
    //     }
    //
    //     var newTask = new ToDoModel
    //     {
    //         title = model.title,
    //         description = model.description,
    //         deadline = model.deadline.HasValue
    //             ? DateTime.SpecifyKind(model.deadline.Value, DateTimeKind.Utc)
    //             : DateTime.UtcNow.Date,
    //         priority = model.priority ?? Priority.MEDIUM,
    //     };
    //
    //     if (model.priority == null &&
    //         _macrosService.CheckMacrosPriority(model.title, Patterns.priorityMacros))
    //     {
    //         newTask.priority = _macrosService.GetPriority(model.title, Patterns.priorityMacros);
    //     }
    //
    //     if (model.deadline == null &&
    //         _macrosService.CheckMacrosDate(model.title, Patterns.dateMacros))
    //     {
    //         var deadline = _macrosService.GetDate(model.title, Patterns.dateMacros);
    //         if (DateTime.SpecifyKind(deadline, DateTimeKind.Utc) <= DateTime.UtcNow)
    //         {
    //             return new ResponseModel(400, "Указанное время не может быть меньше или равно текущему!");
    //         }
    //         newTask.deadline = DateTime.SpecifyKind(deadline, DateTimeKind.Utc);
    //     }
    //
    //     if (newTask.description != null &&
    //         (newTask.description.Length < 15 || newTask.description.Length > 500))
    //     {
    //         return new ResponseModel(400, "Описание либо меньше 15 символов, либо больше 500");
    //     }
    //
    //     if (newTask.deadline <= DateTime.UtcNow)
    //     {
    //         return new ResponseModel(400, "Указанное время не может быть меньше или равно текущему!");
    //     }
    //
    //     newTask.title = _macrosService.deleteMacros(model.title, Patterns.priorityMacros);
    //     newTask.title = _macrosService.deleteMacros(newTask.title, Patterns.dateMacros);
    //     
    //     Console.WriteLine(newTask.deadline);
    //
    //     await _context.Tasks.AddAsync(newTask);
    //     await _context.SaveChangesAsync();
    //
    //     return new ResponseModel(200, "Success");
    // }


    // public async Task<List<TaskPreviewModel>> TaskList(Priority? priority, Sort? sort)
    // {
    //
    //     List<ToDoModel> listModels = new List<ToDoModel>();
    //     List<TaskPreviewModel> listDtoModels = new List<TaskPreviewModel>();
    //     
    //     if (priority != null)
    //     {
    //         if (Enum.TryParse<Priority>(priority, ignoreCase: true, out var parsed) 
    //             && Enum.IsDefined(typeof(Priority), parsed))y
    //         listModels = await _context.Tasks.Where(t => t.priority == priority).ToListAsync();
    //     }
    //     else
    //     {
    //         listModels = await _context.Tasks.ToListAsync();
    //     }
    //     
    //
    //     if (sort != null)
    //     {
    //         switch (sort)
    //         {
    //             case Sort.CreateAsc:
    //                 listModels = listModels.OrderBy(t => t.created_at).ToList();
    //                 break;
    //             case Sort.CreateDesc:
    //                 listModels = listModels.OrderByDescending(t => t.created_at).ToList();
    //                 break;
    //             case Sort.PriorityAsc:
    //                 listModels = listModels.OrderBy(t => t.priority).ToList();
    //                 break;
    //             case Sort.PriorityDesc:
    //                 listModels = listModels.OrderByDescending(t => t.priority).ToList();
    //                 break;
    //             case Sort.StatusAsc:
    //                 listModels = listModels.OrderBy(t => t.status).ToList();
    //                 break;
    //             case Sort.StatusDesc:
    //                 listModels = listModels.OrderByDescending(t => t.status).ToList();
    //                 break;
    //             default:
    //                 break;
    //         }   
    //     }
    //
    //     foreach (var model in listModels)
    //     {
    //         await CheckStatus(model);
    //         var dtoModel = new TaskPreviewModel
    //         {
    //             id = model.id,
    //             title = model.title,
    //             description = model.description,
    //             deadline = model.deadline.HasValue 
    //                 ? DateTime.SpecifyKind(model.deadline.Value, DateTimeKind.Unspecified).AddHours(7) 
    //                 : null,
    //             // deadline = model.deadline,
    //             status = model.status,
    //             priority = model.priority,
    //             created_at = model.created_at,
    //             updatedTime = model.updatedTime
    //         };
    //         listDtoModels.Add(dtoModel);
    //     }
    //
    //     return listDtoModels;
    // }
    public async Task<List<TaskPreviewModel>> TaskList(Priority? priority, Sort? sort)
{
    List<ToDoModel> listModels = new List<ToDoModel>();
    List<TaskPreviewModel> listDtoModels = new List<TaskPreviewModel>();

    if (priority != null)
    {
        listModels = await _context.Tasks
            .Where(t => t.priority == priority)
            .ToListAsync();
    }
    else
    {
        listModels = await _context.Tasks.ToListAsync();
    }

    if (sort != null)
    {
        switch (sort)
        {
            case Sort.CreateAsc:
                listModels = listModels.OrderBy(t => t.created_at).ToList();
                break;
            case Sort.CreateDesc:
                listModels = listModels.OrderByDescending(t => t.created_at).ToList();
                break;
            case Sort.PriorityAsc:
                listModels = listModels.OrderBy(t => t.priority).ToList();
                break;
            case Sort.PriorityDesc:
                listModels = listModels.OrderByDescending(t => t.priority).ToList();
                break;
            case Sort.StatusAsc:
                listModels = listModels.OrderBy(t => t.status).ToList();
                break;
            case Sort.StatusDesc:
                listModels = listModels.OrderByDescending(t => t.status).ToList();
                break;
        }
    }

    foreach (var model in listModels)
    {
        await CheckStatus(model);
        var dtoModel = new TaskPreviewModel
        {
            id = model.id,
            title = model.title,
            description = model.description,
            deadline = model.deadline.HasValue
                ? DateTime.SpecifyKind(model.deadline.Value, DateTimeKind.Unspecified).AddHours(7)
                : null,
            status = model.status,
            priority = model.priority,
            created_at = model.created_at,
            updatedTime = model.updatedTime
        };
        listDtoModels.Add(dtoModel);
    }

    return listDtoModels;
}


    public async Task<ResponseModel> UpdateTask(Guid taskId, UpdateTaskDto model)
    {
        
        var task = await _context.Tasks.FindAsync(taskId);

        if (task == null)
        {
            return new ResponseModel(400, "Такая карточка не существует");
        }
        
        if (model.title == null || model.title.Length < 4)
        {
            return new ResponseModel(400, "Введите название или его длина меньше 4 символов");
        }
        if (model.description != null && (model.description.Length < 15 || model.description.Length > 500))
        {
            return new ResponseModel(400, "Описание либо меньше 15 символов, либо больше 500");
        }
        if (model.deadline?.ToUniversalTime() <= DateTime.UtcNow)
        {
            return new ResponseModel(400, "Указанное время не может бюыть меньше или равно текущему!");
        }
        
        if (model.title.Length < 4 || model.title.Length > 255)
        {
            return new ResponseModel(400, "Название либо больше 3, либо меньше 255");
        }
        
        task.title = model.title;
        if (model.description != null)
        {
            task.description = model.description;
        }

        // task.deadline = model.deadline.HasValue
        //     ? DateTime.SpecifyKind(model.deadline.Value, DateTimeKind.Unspecified).AddHours(7)
        //     : null;
        task.deadline = model.deadline?.ToUniversalTime();
        task.priority = model.priority;
        task.updatedTime = DateTime.UtcNow;

        await CheckStatus(task);
        
        await _context.SaveChangesAsync();

        return new ResponseModel(200, "Success");
    }

    public async Task<ToDoModel> GetTask(Guid taskId)
    {
        var result = await _context.Tasks.FindAsync(taskId);

        if (result != null)
        {
            return result;
        }
        else
        {
            return null;
        }
    }

    public async Task<ResponseModel> DeleteTask(Guid taskId)
    {
        var deletedModel = await _context.Tasks.FindAsync(taskId);

        if (deletedModel == null)
        {
            return new ResponseModel(400, "Такая карточка не существует!");
        }
        else
        {
            _context.Tasks.Remove(deletedModel);
            await _context.SaveChangesAsync();
            return new ResponseModel(200, "Success");
        }
    }

    // public async Task<ResponseModel> UpdateStatus(Guid taskId, UpdateStatusDto model)
    // {
    //     var task = await _context.Tasks.FindAsync(taskId);
    //
    //     if (task == null)
    //     {
    //         return new ResponseModel(404, "Карточка не найдена");
    //     }
    //     else
    //     {
    //         if (!Enum.IsDefined(typeof(Status), model.Status))
    //         {
    //             return new ResponseModel(400, "Такого статуса нет!");
    //         }
    //         if (task.deadline?.ToUniversalTime() <= DateTime.UtcNow && model.Status == Status.COMPLETED)
    //         {
    //             model.Status = Status.LATE;
    //         }
    //
    //         if (task.deadline?.ToUniversalTime() <= DateTime.UtcNow && model.Status == Status.ACTIVE)
    //         {
    //             model.Status = Status.OVERDUE;
    //         }
    //     
    //         task.status = model.Status;
    //         await _context.SaveChangesAsync();
    //     
    //         return new ResponseModel(200, "Success");
    //     }
    // }
    
    public async Task<ResponseModel> UpdateStatus(Guid taskId, UpdateStatusDto model)
    {
        var task = await _context.Tasks.FindAsync(taskId);

        if (task == null)
        {
            return new ResponseModel(400, "Карточка не найдена");
        }

        if (!Enum.IsDefined(typeof(Status), model.Status))
        {
            return new ResponseModel(400, "Такого статуса нет!");
        }

        if (model.Status == Status.OVERDUE || model.Status == Status.LATE)
        {
            return new ResponseModel(400, "Вы не можете поставить этот статус!");
        }

        // Автоматическое изменение статуса в зависимости от дедлайна
        if (task.deadline?.ToUniversalTime() <= DateTime.UtcNow)
        {
            if (model.Status == Status.COMPLETED)
                model.Status = Status.LATE;
            else if (model.Status == Status.ACTIVE)
                model.Status = Status.OVERDUE;
        }

        task.status = model.Status;
        await _context.SaveChangesAsync();

        // Вернуть обновлённые данные
        return new ResponseModel(200, "Успешно обновлён статус задачи");
    }


    private async Task CheckStatus(ToDoModel model)
    {
        var task = await _context.Tasks.FindAsync(model.id);
        if (DateTime.UtcNow >= model.deadline && model.status == Status.ACTIVE)
        {
            model.status = Status.OVERDUE;
            await _context.SaveChangesAsync();
        }
        else if (!(DateTime.UtcNow >= model.deadline) && model.status != Status.ACTIVE)
        {
            model.status = Status.ACTIVE;
            await _context.SaveChangesAsync();
        }
    }
}