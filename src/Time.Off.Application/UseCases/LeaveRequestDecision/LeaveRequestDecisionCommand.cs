using Time.Off.Domain.Enums;

namespace Time.Off.Application.UseCases.LeaveRequestDecision;

public record LeaveRequestDecisionCommand(
    LeaveRequestStatus Decision,
    string? ManagerComment = null
);
