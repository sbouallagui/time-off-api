using Time.Off.Domain.Enums;
using Time.Off.Domain.ValueObjects;

namespace Time.Off.Domain.Entities;

public class LeaveRequest
{
    public Guid Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public LeavePeriod? Period { get; private set; }
    public LeaveType Type { get; private set; }
    public string? Comment { get; private set; }
    public LeaveRequestStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ModifiedAt { get; private set; }
    public string? ManagerComment { get; private set; }

    // Creation constructor
    public LeaveRequest(Guid employeeId, LeavePeriod period, LeaveType type, string? comment = null)
    {
        ArgumentNullException.ThrowIfNull(period);

        Id = Guid.NewGuid();
        EmployeeId = employeeId;
        Period = period;
        Type = type;
        Comment = comment;
        Status = LeaveRequestStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    // Parameterless constructor for ORM or serialization
    public LeaveRequest() { }

    public void Approve(string? managerComment = null)
    {
        EnsureStatusIsPending("approved");
        Status = LeaveRequestStatus.Approved;
        ManagerComment = managerComment;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Reject(string? managerComment = null)
    {
        EnsureStatusIsPending("rejected");
        Status = LeaveRequestStatus.Rejected;
        ManagerComment = managerComment;
        ModifiedAt = DateTime.UtcNow;
    }

    private void EnsureStatusIsPending(string action)
    {
        if (Status != LeaveRequestStatus.Pending)
            throw new InvalidOperationException($"Only pending requests can be {action}.");
    }
}
