using Time.Off.Domain.Enums;

namespace Time.Off.Application.UseCases.SubmitLeaveRequest
{
    public record RequestLeaveCommand(
        Guid EmployeeId,
        DateOnly StartDate,
        DateOnly EndDate,
        LeaveType Type,
        string? Comment);
}
