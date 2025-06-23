using Time.Off.Domain.Enums;

namespace Time.Off.Infrastructure.Dtos;

public class LeaveRequestDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public LeaveType Type { get; set; }
    public string? Comment { get; set; }
    public LeaveRequestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
