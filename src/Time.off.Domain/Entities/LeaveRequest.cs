namespace Time.off.Domain;

public class LeaveRequest
{
    public Guid Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public LeavePeriod Period { get; private set; }
    public LeaveType Type { get; private set; }
    public string? Comment { get; private set; }
    public LeaveRequestStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Constructeur de création
    public LeaveRequest(Guid employeeId, LeavePeriod period, LeaveType type, string? comment = null)
    {
        if (period == null) throw new ArgumentNullException(nameof(period));

        Id = Guid.NewGuid();
        EmployeeId = employeeId;
        Period = period;
        Type = type;
        Comment = comment;
        Status = LeaveRequestStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    // Méthodes métier possibles (ex : Annuler)
    public void Cancel()
    {
        if (Status != LeaveRequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be cancelled.");

        Status = LeaveRequestStatus.Cancelled;
    }
}
