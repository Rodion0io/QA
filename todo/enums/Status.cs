using System.Text.Json.Serialization;

namespace todo.enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    ACTIVE,
    COMPLETED,
    OVERDUE,
    LATE
}