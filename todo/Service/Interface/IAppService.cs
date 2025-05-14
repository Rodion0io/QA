using todo.DTO;
using todo.enums;
using todo.Models;

namespace todo.Service.Interface;

public interface IAppService
{
    public Task<ResponseModel> NewTask(CreateTaskRequestDto model);

    public Task<List<TaskPreviewModel>> TaskList(Priority? priority, Sort? sort);
    
    public Task<ResponseModel> UpdateTask(Guid taskId, UpdateTaskDto model);

    public Task<ToDoModel> GetTask(Guid taskId);
    
    public Task<ResponseModel> DeleteTask(Guid taskId);
    
    public Task<ResponseModel> UpdateStatus(Guid taskId, UpdateStatusDto model);
}