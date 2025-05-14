using todo.enums;

namespace todo.DTO;

public class TaskPreviewModel
{
    public Guid id { get; set; }
    
    public string title { get; set; }
    
    public string? description { get; set; }
    
    public DateTime? deadline { get; set; }
    
    public Status status { get; set; }
    
    public Priority priority { get; set; }
    
    public DateTime created_at { get; set; }
    
    public DateTime? updatedTime { get; set; }
}