using System.ComponentModel.DataAnnotations;
using todo.enums;

namespace todo.DTO;

public class UpdateTaskDto
{
    [Required]
    [MinLength(4)]
    public string title { get; set; }
    
    [MinLength(15)]
    [MaxLength(500)]
    public string? description { get; set; }
    
    public DateTime? deadline { get; set; }
    
    public Priority priority { get; set; }
}