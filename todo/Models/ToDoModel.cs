using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using todo.enums;

namespace todo.Models;

public class ToDoModel
{

    public ToDoModel()
    {
        id = Guid.NewGuid();
        status = Status.ACTIVE;
        priority = Priority.MEDIUM;
        deadline = null;
        created_at = DateTime.UtcNow;
        updatedTime = null;
    }
    
    [Required]
    public Guid id { get; set; }
    
    [Required]
    [MinLength(4)]
    public string title { get; set; }
    
    [MinLength(15)]
    [MaxLength(500)]
    public string? description { get; set; }
    
    public DateTime? deadline { get; set; }
    
    public Status status { get; set; }
    
    public Priority priority { get; set; }
    
    [JsonIgnore]
    public DateTime created_at { get; set; }
    
    [JsonIgnore]
    public DateTime? updatedTime { get; set; }
    
    
}