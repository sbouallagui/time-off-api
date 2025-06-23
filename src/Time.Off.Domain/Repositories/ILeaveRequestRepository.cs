using Time.Off.Domain.Entities;

namespace Time.Off.Domain.Repositories;

public interface ILeaveRequestRepository
{
    Task<Guid> AddAsync(LeaveRequest leaveRequest);
    Task<LeaveRequest?> GetByIdAsync(Guid id);
}
