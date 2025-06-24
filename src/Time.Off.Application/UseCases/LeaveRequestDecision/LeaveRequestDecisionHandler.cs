
using FluentValidation;
using Microsoft.Extensions.Logging;
using Time.Off.Application.Common;
using Time.Off.Domain.Enums;
using Time.Off.Domain.Repositories;

namespace Time.Off.Application.UseCases.LeaveRequestDecision;

public class LeaveRequestDecisionHandler(
    ILeaveRequestRepository repository,
    IValidator<LeaveRequestDecisionCommand> validator,
    ILogger<LeaveRequestDecisionHandler> logger)
{
    private readonly ILeaveRequestRepository _repository = repository;
    private readonly IValidator<LeaveRequestDecisionCommand> _validator = validator;
    private readonly ILogger<LeaveRequestDecisionHandler> _logger = logger;

    public async Task<OperationResult<bool>> HandleAsync(Guid leaveRequestId, LeaveRequestDecisionCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return OperationResult<bool>.Failure(errors);
        }

        var leaveRequest = await _repository.GetByIdAsync(leaveRequestId);
        if (leaveRequest == null)
            return OperationResult<bool>.Failure("Leave request not found.");

        try
        {
            switch (command.Decision)
            {
                case LeaveRequestStatus.Approved:
                    leaveRequest.Approve(command.ManagerComment);
                    break;

                case LeaveRequestStatus.Rejected:
                    leaveRequest.Reject(command.ManagerComment);
                    break;

                default:
                    return OperationResult<bool>.Failure("Invalid decision status.");
            }

            await _repository.UpdateAsync(leaveRequest);
            return OperationResult<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process decision for leave request {LeaveRequestId}", leaveRequestId);
            return OperationResult<bool>.Failure("An error occurred while processing the decision.");
        }
    }

}
