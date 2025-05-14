using System.Globalization;
using todo.DTO;
using todo.enums;

namespace todo.test.api_test.testDatas;

public class NewTaskTestDatasValid
{
    public static IEnumerable<object[]> NewTaskDatas()
    {
        yield return new object[]
        {
            new CreateTaskRequestDto(
                "tsdladngflksaitle !before 20.12.2025", "descrijnlsfdlkjgnsdkjzfnption", null, Priority.LOW)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto("title !1", null, null, Priority.MEDIUM)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto("titl", "qqqqqqqqqqqqqqqqq",
                DateTime.SpecifyKind(DateTimeOffset.Parse("2025-12-31T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc),
                null)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto("!3titldsjkrzgnkejsendkjf !before 30.05.2025",
                null, DateTime.SpecifyKind(DateTimeOffset.Parse("2150-10-10T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc),
                Priority.HIGH)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto("titldsjkrzgnkejsendkjf", "doasgnilsdjkhzfnijsdzgnilsfj",
                DateTime.SpecifyKind(DateTimeOffset.Parse("2025-06-30T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc),
                Priority.CRITICAL)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto("titldsjkrzgnkejsendkjf", "doasgnilsdjkhzfnijsdzgnilsfj",
                DateTime.SpecifyKind(DateTimeOffset.Parse("2025-05-13T21:21:58.474Z").UtcDateTime, DateTimeKind.Utc),
                Priority.CRITICAL)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto("titldsjkrzgnkejsendkjf", null,
                null,
                null)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto("titldsjkrzgnkejsendkjf !3", null,
                null,
                null)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto(new string('t', 255), null,
                null,
                null)
        };
    }
    
    public static IEnumerable<object[]> NewTaskDatasNotValid()
    {
        yield return new object[]
        {
            new CreateTaskRequestDto(
                "t", "descrijnlsfdlkjgnsdkjzfnption", null, Priority.LOW)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto(
                null, "descrijnlsfdlkjgnsdkjzfnption", null, Priority.HIGH)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto(
                "skjdgnkjfsd", "desc", null, Priority.CRITICAL)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto(
                "skjdgnkjfsd ", null,
                DateTime.SpecifyKind(DateTimeOffset.Parse("2012-05-13T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc),
                Priority.MEDIUM)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto(
                "skjdgnkjfsd", null,
                DateTime.SpecifyKind(DateTimeOffset.Parse("2000-05-13T09:07:58.474Z").UtcDateTime, DateTimeKind.Utc),
                Priority.LOW)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto(
                "skjdgnkjfsd", null, DateTime.SpecifyKind(DateTimeOffset.Parse("2012-05-13T15:07:58.474Z").UtcDateTime, DateTimeKind.Utc), (Priority)(-1))
        };
        yield return new object[]
        {
            new CreateTaskRequestDto(
                "skjdgnkjfsd", null, DateTime.SpecifyKind(DateTimeOffset.Parse("2025-05-13T00:07:58.474Z").UtcDateTime, DateTimeKind.Utc), Priority.LOW)
        };
        yield return new object[]
        {
            new CreateTaskRequestDto(
                "skjdgnkjfsd", null, DateTime.SpecifyKind(DateTimeOffset.Parse("2025-05-13T00:07:58.474Z").UtcDateTime, DateTimeKind.Utc), Priority.LOW)
        };
    }
}
