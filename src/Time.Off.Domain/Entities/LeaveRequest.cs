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

    // Constructeur de création
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

    public LeaveRequest() { }

    public void Cancel()
    {
        if (Status != LeaveRequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be cancelled.");

        Status = LeaveRequestStatus.Cancelled;
    }
}
