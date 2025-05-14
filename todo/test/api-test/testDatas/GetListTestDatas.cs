using todo.enums;

namespace todo.test.api_test.testDatas;

public class GetListTestDatas
{
    public static IEnumerable<object[]> ValidUrlData()
    {
        yield return new object[] { "HIGH", null };
        yield return new object[] { "CRITICAL", "CreateAsc" };
        yield return new object[] { "MEDIUM", "CreateDesc" };
        yield return new object[] { "LOW", null };
        yield return new object[] { "HIGH", "PriorityAsc" };
        yield return new object[] { null, "PriorityDesc" };
        yield return new object[] { "CRITICAL", null };
        yield return new object[] { null, null };
        yield return new object[] { "", "" };
    }

    public static IEnumerable<object[]> InValidUrlData()
    {
        yield return new object[] { "CreateAsc", "CRITICAL" };
        yield return new object[] { "", "CRITICAL" };
        yield return new object[] { "1323", "+++(.%" };
        yield return new object[] { "HУGH", "CreateAsc" };
        yield return new object[] { "20.12.2025", "CreateAsc" };
        yield return new object[] { "LOW", "bimbimbambam" };
        yield return new object[] { "CRITICAL_PRIORITY", "bimbimbambam" };
        yield return new object[] { "LOW", " C r e a t e A s c " };
    }
}