using todo.DTO;
using todo.enums;

namespace todo.test.api_test.testDatas;

public class UpdateStatusDatas
{
    public static IEnumerable<object[]> UpdateStatusTaskDatas()
    {
        yield return new object[]
        {
            Guid.Parse("02a652c1-d03e-4675-a73e-36440057bcff"),
            new UpdateStatusDto
            {
                Status = Status.ACTIVE
            }
        };
        yield return new object[]
        {
            Guid.Parse("0d25260b-427e-47cd-85b1-1963c84b668b"),
            new UpdateStatusDto
            {
                Status = Status.COMPLETED
            }
        };
    }
    
    public static IEnumerable<object[]> UpdateStatusTaskDatasNotValid()
    {
        yield return new object[]
        {
            Guid.Parse("36356ebb-7bf1-417b-be5a-294cce7e3679"),
            new UpdateStatusDto
            {
                Status = Status.OVERDUE
            }
            
        };
        yield return new object[]
        {
            Guid.Parse("363566bb-7bf1-417b-be5a-294bce7e3679"),
            new UpdateStatusDto
            {
                Status = Status.LATE
            }
            
        };
        yield return new object[]
        {
            Guid.Parse("363566bb-7bf1-417b-be5a-294bce7e3679"),
            new UpdateStatusDto
            {
                Status = (Status)(-1)
            }
        };
        yield return new object[]
        {
            Guid.Parse("10750fb4-128c-4169-ad53-4fd7c18d8ac1"),
            new UpdateStatusDto
            {
                Status = Status.COMPLETED
            }
        };
    }
}