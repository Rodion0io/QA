using Microsoft.AspNetCore.Mvc;
using todo.DTO;
using todo.enums;
using todo.Models;
using todo.Service.Interface;

namespace todo.controllers;

[ApiController]
[Route("api/[controller]")]
public class AppController : Controller
{
    
    private readonly IAppService _appService;

    public AppController(IAppService appService)
    {
        _appService = appService;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequestDto model)
    {
        if (model == null)
        {
            return BadRequest("Пустой тело запроса");
        }
        else
        {
            var result = await _appService.NewTask(model);
            if (result.statusCode == 400)
            {
                return BadRequest(result);
            }
            else if (result.statusCode == 201)
            {
                return Ok(result);
            }
        }

        return Ok("Заглушка");
    }

    // [HttpGet("tasks")]
    // public async Task<IActionResult> GetTasks([FromQuery] Priority? priority,
    //     [FromQuery] Sort? sort)
    // {
    //     
    //     
    //     var listTasks = await _appService.TaskList(priority, sort);
    //     
    //     return Ok(listTasks);
    // }
    
    [HttpGet("tasks")]
    public async Task<IActionResult> GetTasks([FromQuery] string? priority, [FromQuery] string? sort)
    {
        Priority? parsedPriority = null;
        Sort? parsedSort = null;

        if (priority != null)
        {
            if (Enum.TryParse(priority, ignoreCase: true, out Priority tempPriority) && Enum.IsDefined(typeof(Priority), tempPriority))
            {
                parsedPriority = tempPriority;
            }
            else
            {
                return BadRequest("Такого приоритета нет");
            }
        }

        if (sort != null)
        {
            if (Enum.TryParse(sort, ignoreCase: true, out Sort tempSort) && Enum.IsDefined(typeof(Sort), tempSort))
            {
                parsedSort = tempSort;
            }
            else
            {
                return BadRequest("Такого вида сортировки нет");
            } 
        }

        var listTasks = await _appService.TaskList(parsedPriority, parsedSort);
        return Ok(listTasks);
    }






    [HttpPut("update/{taskId}")]
    public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] UpdateTaskDto model)
    {
        if (model == null)
        {
            return BadRequest("Пустой тело запроса");
        }
        else
        {
            var result = await _appService.UpdateTask(taskId, model);
            if (result.statusCode == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }

    [HttpGet("getTask/{taskId}")]
    public async Task<IActionResult> GetTask(Guid taskId)
    {
        var task = await _appService.GetTask(taskId);

        if (task == null)
        {
            ResponseModel errorModel = new ResponseModel(400, "Такой карточки не существует!");
            return BadRequest(errorModel);
        }
        else
        {
            return Ok(task);
        }
    }

    [HttpDelete("deleteTask/{taskId}")]
    public async Task<IActionResult> DeleteTask(Guid taskId)
    {
        var result = await _appService.DeleteTask(taskId);

        if (result.statusCode == 200)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }

    [HttpPut("updateStatus/{taskId}")]
    public async Task<IActionResult> UpdateStatus(Guid taskId, [FromBody] UpdateStatusDto model)
    {
        var result = await _appService.UpdateStatus(taskId, model);

        if (result.statusCode == 200)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }
}