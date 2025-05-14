using System.Text.Json.Serialization;

namespace todo.enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Sort
{
    CreateAsc,
    CreateDesc,
    PriorityAsc,
    PriorityDesc,
    StatusAsc,
    StatusDesc,
}