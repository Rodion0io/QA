namespace todo.test.api_test.testDatas;

public class DeleteTaskDatas
{
    public static IEnumerable<object[]> ValidUrlData()
    {
        yield return new object[] { Guid.Parse("d8787530-d5fe-4e31-aff9-1cce2b7d8388") };
    }
    
    public static IEnumerable<object[]> NotValidUrlData()
    {
        yield return new object[] { Guid.Parse("d8787530-d5fe-4e31-aff9-1uze2b7d8388") };
    }
}