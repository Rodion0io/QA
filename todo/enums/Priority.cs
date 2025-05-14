using System.Text.Json.Serialization;

namespace todo.enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Priority
{
    CRITICAL,
    HIGH,
    MEDIUM,
    LOW
}