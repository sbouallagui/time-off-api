using Time.Off.Domain.Entities;
using Time.Off.Domain.ValueObjects;

namespace Time.Off.Domain.Repositories;

public interface ILeaveRequestRepository
{
    Task<Guid> AddAsync(LeaveRequest leaveRequest);
    Task<LeaveRequest?> GetByIdAsync(Guid id);
    Task<bool> ExistsPendingRequestForPeriodAsync(Guid employeeId, LeavePeriod period);
    Task UpdateAsync(LeaveRequest leaveRequest);
}
