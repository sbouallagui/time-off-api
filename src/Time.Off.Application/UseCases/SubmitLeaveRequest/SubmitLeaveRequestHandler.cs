using FluentValidation;
using Time.Off.Application.Common;
using Time.Off.Domain.Entities;
using Time.Off.Domain.Repositories;
using Time.Off.Domain.ValueObjects;


namespace Time.Off.Application.UseCases.SubmitLeaveRequest
{
    public class SubmitLeaveRequestHandler(ILeaveRequestRepository repository, IValidator<RequestLeaveCommand> validator)
    {
        private readonly ILeaveRequestRepository _repository = repository;
        private readonly IValidator<RequestLeaveCommand> _validator = validator;

        public async Task<OperationResult<Guid>> HandleAsync(RequestLeaveCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return OperationResult<Guid>.Failure(errors);
            }

            var period = new LeavePeriod(command.StartDate, command.EndDate);
            var leaveRequest = new LeaveRequest(command.EmployeeId, period, command.Type, command.Comment);

            var lastInsertedId = await _repository.AddAsync(leaveRequest);

            return OperationResult<Guid>.Success(lastInsertedId);
        }
    }
}
