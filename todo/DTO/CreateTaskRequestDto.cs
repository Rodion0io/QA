using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using todo.enums;

namespace todo.DTO;

public record CreateTaskRequestDto(
    [Required]
    [MinLength(4)]
    string title,
    
    [MinLength(15)]
    [MaxLength(500)]
    string? description,
    
    DateTime? deadline,
    
    Priority? priority
    );