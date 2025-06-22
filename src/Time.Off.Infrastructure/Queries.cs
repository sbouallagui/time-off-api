namespace Time.Off.Infrastructure;

public static class Queries
{
    public const string InsertIntoLeaveRequest = @"
    INSERT INTO LeaveRequest
        (Id, EmployeeId, StartDate, EndDate, Type, Comment, Status, CreatedAt, ModifiedAt)
    VALUES
        (@Id, @EmployeeId, @StartDate, @EndDate, @Type::leavetype, @Comment, @Status::leaverequeststatus, @CreatedAt, @ModifiedAt)
    RETURNING Id;
    ";
}
