using Time.Off.Domain.Enums;

namespace Time.Off.Application.UseCases.SubmitLeaveRequest
{
    public record RequestLeaveCommand(
        Guid EmployeeId,
        DateTime StartDate,
        DateTime EndDate,
        LeaveType Type,
        string? Comment);
}
