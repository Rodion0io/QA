namespace todo.Models;

public record ResponseModel(
    int statusCode,
    string message
    )
{
    // public int statusCode { get; set; }
    // public string message { get; set; }
}