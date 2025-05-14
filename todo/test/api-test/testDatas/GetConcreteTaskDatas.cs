namespace todo.test.api_test.testDatas;

public class GetConcreteTaskDatas
{
    public static IEnumerable<object[]> ValidUrlData()
    {
        yield return new object[] { Guid.Parse("02a652c1-d03e-4675-a73e-36440057bcff") };
        yield return new object[] { "0d25260b-427e-47cd-85b1-1963c84b668b" };
    }
    
    public static IEnumerable<object[]> NotValidUrlData()
    {
        yield return new object[] { Guid.Parse("0d25261b-427e-47cd-85b1-1963c84b668b") };
        yield return new object[] { "0d25261b-427e-47cd-85b1-1963c84b698b" };
    }
}