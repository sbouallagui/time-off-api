using Time.off.Domain.Entities;

namespace Time.off.Domain.Repositories;

public interface ILeaveRequestRepository
{
    Task AddAsync(LeaveRequest leaveRequest);
}
