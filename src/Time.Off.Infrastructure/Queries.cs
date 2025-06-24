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

    public const string GetLeaveRequestById = @"
    SELECT 
        Id,
        EmployeeId,
        Type::text AS Type,
        Comment,
        Status::text AS Status,
        CreatedAt,
        ModifiedAt,
        StartDate,
        EndDate
    FROM LeaveRequest
    WHERE Id = @Id;
    ";

    public const string ExistsPendingRequestForPeriodQuery = @"
        SELECT 1
        FROM LeaveRequest
        WHERE EmployeeId = @EmployeeId
          AND Status = 'Pending'
          AND StartDate = @StartDate
          AND EndDate = @EndDate
        LIMIT 1
    ";
}
