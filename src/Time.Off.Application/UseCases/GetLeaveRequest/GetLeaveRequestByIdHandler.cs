using Time.Off.Application.Common;
using Time.Off.Domain.Entities;
using Time.Off.Domain.Repositories;

namespace Time.Off.Application.UseCases.GetLeaveRequest;

public class GetLeaveRequestByIdHandler(ILeaveRequestRepository repository)
{
    private readonly ILeaveRequestRepository _repository = repository;

    public async Task<OperationResult<LeaveRequest?>> HandleAsync(GetLeaveRequestByIdQuery query)
    {
        var leaveRequest = await _repository.GetByIdAsync(query.RequestId);

        if (leaveRequest == null)
            return OperationResult<LeaveRequest?>.Failure("Leave request not found");

        return OperationResult<LeaveRequest?>.Success(leaveRequest);
    }
}
