using Time.Off.Domain.Entities;

namespace Time.Off.Domain.Repositories;

public interface ILeaveRequestRepository
{
    public Task<Guid> AddAsync(LeaveRequest leaveRequest);
}
