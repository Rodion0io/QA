using todo.DTO;
using todo.enums;

namespace todo.test.api_test.testDatas;

public class UpdateTaskTestDatas
{
    public static IEnumerable<object[]> UpdateTaskDatas()
    {
        yield return new object[]
        {
            Guid.Parse("02a652c1-d03e-4675-a73e-36440057bcff"),
            new UpdateTaskDto
            {
                title = "1234 !3",
                description = null,
                deadline = null,
                priority = Priority.LOW
            }
        };

        yield return new object[]
        {
            Guid.Parse("0d25260b-427e-47cd-85b1-1963c84b668b"),
            new UpdateTaskDto
            {
                title = "бимбимбамбам !before 10.10.2025",
                description = "123456789012345",
                deadline = null,
                priority = Priority.MEDIUM
            }
        };
        
        yield return new object[]
        {
            Guid.Parse("10750cb4-128c-4569-ad53-4fd7c18d8fc1"),
            new UpdateTaskDto
            {
                title = "kjdrbfjkhebfjkwe",
                description = "qqqqqqqqqqqqqqqqq",
                deadline = DateTime.SpecifyKind(DateTime.Parse("2025-05-14T09:07:58.474Z"), DateTimeKind.Utc),
                priority = Priority.HIGH
            }
        };
        yield return new object[]
        {
            Guid.Parse("2a8f465d-7917-444d-8564-8c3fe464ea57"),
            new UpdateTaskDto
            {
                title = "update",
                description = null,
                deadline = DateTime.SpecifyKind(DateTime.Parse("2025-05-14T09:07:58.474Z"), DateTimeKind.Utc),
                priority = Priority.CRITICAL
            }
        };
        yield return new object[]
        {
            Guid.Parse("4ea43e6d-64be-4ca4-8726-17a162738fdc"),
            new UpdateTaskDto
            {
                title = "update",
                description = null,
                deadline = DateTime.SpecifyKind(DateTime.Parse("2500-12-31T09:07:58.474Z"), DateTimeKind.Utc),
                priority = Priority.CRITICAL
            }
        };
        
        yield return new object[]
        {
            Guid.Parse("4ea43e6d-64be-4ca4-8726-17a162738fdc"),
            new UpdateTaskDto
            {
                title = "update !before 10-10-2025 !3",
                description = null,
                deadline = DateTime.SpecifyKind(DateTime.Parse("2500-12-31T09:07:58.474Z"), DateTimeKind.Utc),
                priority = Priority.CRITICAL
            }
        };
    }
    
    public static IEnumerable<object[]> UpdateTaskDatasNotValid()
    {
        yield return new object[]
        {
            Guid.Parse("7700d3d2-28c9-4202-8468-9c594401168a"),
            new UpdateTaskDto
            {
                title = "123",
                description = null,
                deadline = null,
                priority = Priority.LOW
            }
        };

        yield return new object[]
        {
            Guid.Parse("7700d3d2-28c9-4202-8468-9c594402168a"),
            new UpdateTaskDto
            {
                title = "",
                description = "1387923874928374928032",
                deadline = DateTime.SpecifyKind(DateTime.Parse("2025-05-13T20:20:58.474Z"), DateTimeKind.Utc),
                priority = Priority.MEDIUM
            }
        };
        
        yield return new object[]
        {
            Guid.Parse("7700d3d2-28c9-4202-8468-9c594402168a"),
            new UpdateTaskDto
            {
                title = "kjdrbfjkhebfjkwe",
                description = "12345678901234",
                deadline = DateTime.SpecifyKind(DateTime.Parse("2025-05-14T09:07:58.474Z"), DateTimeKind.Utc),
                priority = Priority.HIGH
            }
        };
        yield return new object[]
        {
            Guid.Parse("7700d3d2-28c9-4202-8468-9c594402168a"),
            new UpdateTaskDto
            {
                title = "update",
                description = null,
                deadline = DateTime.SpecifyKind(DateTime.Parse("2024-05-14T09:07:58.474Z"), DateTimeKind.Utc),
                priority = Priority.CRITICAL
            }
        };
        yield return new object[]
        {
            Guid.Parse("7700d3d2-28c9-4202-8468-9c594402168a"),
            new UpdateTaskDto
            {
                title = "update",
                description = null,
                deadline = DateTime.SpecifyKind(DateTime.Parse("2025-05-13T00:00:58.474Z"), DateTimeKind.Utc),
                priority = Priority.CRITICAL
            }
        };
        yield return new object[]
        {
            Guid.Parse("7700d3d2-28c9-4202-8468-9c594402168a"),
            new UpdateTaskDto
            {
                title = "update",
                description = null,
                deadline = DateTime.SpecifyKind(DateTime.Parse("2025-05-13T00:00:58.474Z"), DateTimeKind.Utc),
                priority = Priority.CRITICAL
            }
        };
        yield return new object[]
        {
            Guid.Parse("7700d3d2-28c9-4202-8468-9c594402168a"),
            new UpdateTaskDto
            {
                title = "update",
                description = null,
                deadline = DateTime.SpecifyKind(DateTime.Parse("2025-05-13T00:00:58.474Z"), DateTimeKind.Utc),
                priority = Priority.CRITICAL
            }
        };
        yield return new object[]
        {
            Guid.Parse("7700d3d2-28c9-4202-8468-9c594402168a"),
            new UpdateTaskDto
            {
                title = "update",
                description = null,
                deadline = DateTime.SpecifyKind(DateTime.Parse("2025-05-12T00:00:58.474Z"), DateTimeKind.Utc),
                priority = Priority.CRITICAL
            }
        };
        
        yield return new object[]
        {
            Guid.Parse("7700d3d2-28c9-4202-8468-9c594402168a"),
            new UpdateTaskDto
            {
                title = "update",
                description = null,
                deadline = DateTime.SpecifyKind(DateTime.Parse("2024-05-14T09:07:58.474Z"), DateTimeKind.Utc),
                priority = (Priority)(-1)
            }
        };
    }
}