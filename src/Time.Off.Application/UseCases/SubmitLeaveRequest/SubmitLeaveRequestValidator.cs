namespace Time.Off.Application.UseCases.SubmitLeaveRequest;

using FluentValidation;

public class SubmitLeaveRequestValidator : AbstractValidator<RequestLeaveCommand>
{
    public SubmitLeaveRequestValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Start date must be before or equal to end date.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid leave type.");
    }
}

