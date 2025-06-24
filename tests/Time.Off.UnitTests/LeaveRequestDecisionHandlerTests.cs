using FluentValidation.Results;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Time.Off.Application.UseCases.LeaveRequestDecision;
using Time.Off.Domain.Entities;
using Time.Off.Domain.Enums;
using Time.Off.Domain.Repositories;
using Time.Off.Domain.ValueObjects;
using FluentAssertions;

namespace Time.Off.UnitTests;

public class LeaveRequestDecisionHandlerTests
{
    private readonly Mock<ILeaveRequestRepository> _repositoryMock;
    private readonly Mock<IValidator<LeaveRequestDecisionCommand>> _validatorMock;
    private readonly Mock<ILogger<LeaveRequestDecisionHandler>> _loggerMock;
    private readonly LeaveRequestDecisionHandler _handler;

    public LeaveRequestDecisionHandlerTests()
    {
        _repositoryMock = new Mock<ILeaveRequestRepository>();
        _validatorMock = new Mock<IValidator<LeaveRequestDecisionCommand>>();
        _loggerMock = new Mock<ILogger<LeaveRequestDecisionHandler>>();
        _handler = new LeaveRequestDecisionHandler(_repositoryMock.Object, _validatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Failure_When_Validation_Fails()
    {
        // Arrange
        var command = new LeaveRequestDecisionCommand(LeaveRequestStatus.Approved, "Good");
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Decision", "Invalid decision") });

        _validatorMock.Setup(v => v.ValidateAsync(command, default)).ReturnsAsync(validationResult);

        // Act
        var result = await _handler.HandleAsync(Guid.NewGuid(), command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Invalid decision");
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Failure_When_LeaveRequest_Not_Found()
    {
        // Arrange
        var command = new LeaveRequestDecisionCommand(LeaveRequestStatus.Approved, "Good");
        _validatorMock.Setup(v => v.ValidateAsync(command, default)).ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((LeaveRequest?)null);

        // Act
        var result = await _handler.HandleAsync(Guid.NewGuid(), command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Leave request not found");
    }

    [Fact]
    public async Task HandleAsync_Should_Approve_Request_When_Valid()
    {
        // Arrange
        var leaveRequest = new LeaveRequest(Guid.NewGuid(), new LeavePeriod(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))), LeaveType.PaidLeave);
        var command = new LeaveRequestDecisionCommand(LeaveRequestStatus.Approved, "OK");

        _validatorMock.Setup(v => v.ValidateAsync(command, default)).ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(leaveRequest);
        _repositoryMock.Setup(r => r.UpdateAsync(leaveRequest)).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.HandleAsync(leaveRequest.Id, command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        leaveRequest.Status.Should().Be(LeaveRequestStatus.Approved);
        leaveRequest.ManagerComment.Should().Be("OK");
        _repositoryMock.Verify(r => r.UpdateAsync(leaveRequest), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_Reject_Request_When_Valid()
    {
        // Arrange
        var leaveRequest = new LeaveRequest(
            Guid.NewGuid(),
            new LeavePeriod(DateOnly.FromDateTime(DateTime.UtcNow),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))),
            LeaveType.PaidLeave);
        var command = new LeaveRequestDecisionCommand(
            LeaveRequestStatus.Rejected, "Not acceptable");

        _validatorMock.Setup(v => v.ValidateAsync(command, default)).ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(leaveRequest);
        _repositoryMock.Setup(r => r.UpdateAsync(leaveRequest)).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.HandleAsync(leaveRequest.Id, command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        leaveRequest.Status.Should().Be(LeaveRequestStatus.Rejected);
        leaveRequest.ManagerComment.Should().NotBeNull();
        _repositoryMock.Verify(r => r.UpdateAsync(leaveRequest), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Failure_When_Decision_Is_Invalid()
    {
        // Arrange
        var leaveRequest = new LeaveRequest(Guid.NewGuid(), new LeavePeriod(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))), LeaveType.PaidLeave);
        var command = new LeaveRequestDecisionCommand((LeaveRequestStatus)999, "???");

        _validatorMock.Setup(v => v.ValidateAsync(command, default)).ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(leaveRequest);

        // Act
        var result = await _handler.HandleAsync(leaveRequest.Id, command);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}
