using FluentValidation;
using Time.Off.Domain.Enums;

namespace Time.Off.Application.UseCases.LeaveRequestDecision;

public class LeaveRequestDecisionCommandValidator : AbstractValidator<LeaveRequestDecisionCommand>
{
    public LeaveRequestDecisionCommandValidator()
    {
        RuleFor(x => x.Decision)
            .Must(decision => decision == LeaveRequestStatus.Approved || decision == LeaveRequestStatus.Rejected)
            .WithMessage("Decision must be either 'Approved' or 'Rejected'.");

        RuleFor(x => x.ManagerComment)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.ManagerComment))
            .WithMessage("ManagerComment must be at most 500 characters.");
    }
}
