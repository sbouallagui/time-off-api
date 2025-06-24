using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Time.Off.Application.UseCases.SubmitLeaveRequest;
using Time.Off.Domain.Entities;
using Time.Off.Domain.Enums;
using Time.Off.Domain.Repositories;
using Time.Off.Domain.ValueObjects;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Time.Off.UnitTests;

public class SubmitLeaveRequestHandlerTests
{
    private readonly Mock<ILeaveRequestRepository> _repositoryMock;
    private readonly Mock<IValidator<RequestLeaveCommand>> _validatorMock;
    private readonly SubmitLeaveRequestHandler _handler;

    public SubmitLeaveRequestHandlerTests()
    {
        _repositoryMock = new Mock<ILeaveRequestRepository>();
        _validatorMock = new Mock<IValidator<RequestLeaveCommand>>();
        _handler = new SubmitLeaveRequestHandler(_repositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Success_When_Valid_Request()
    {
        // Arrange
        var command = new RequestLeaveCommand(
            Guid.NewGuid(),
            DateOnly.FromDateTime(DateTime.UtcNow),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)),
            LeaveType.PaidLeave,
            "Test request"
        );

        var newRequestId = Guid.NewGuid();

        _validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<LeaveRequest>()))
            .ReturnsAsync(newRequestId);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(newRequestId);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<LeaveRequest>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Failure_When_Validation_Fails()
    {
        // Arrange
        var command = new RequestLeaveCommand(
            Guid.NewGuid(),
            DateOnly.FromDateTime(DateTime.UtcNow),
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
            LeaveType.PaidLeave,
            "Invalid request"
        );

        var validationErrors = new ValidationResult(
        [
                new ValidationFailure("StartDate", "StartDate must be before EndDate")
        ]);

        _validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationErrors);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("StartDate must be before EndDate");
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<LeaveRequest>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Failure_When_Pending_Request_Exists_For_Same_Period()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var startDate = new DateOnly(2024, 06, 23);
        var endDate = new DateOnly(2024, 06, 28);

        var command = new RequestLeaveCommand(
            employeeId,
            startDate,
            endDate,
            LeaveType.PaidLeave,
            "Overlap test"
        );

        _validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock
            .Setup(r => r.ExistsPendingRequestForPeriodAsync(employeeId, It.IsAny<LeavePeriod>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().Be(Guid.Empty);

        _repositoryMock.Verify(r => r.ExistsPendingRequestForPeriodAsync(employeeId, It.IsAny<LeavePeriod>()), Times.Once);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<LeaveRequest>()), Times.Never); // ensure no insert happened
    }

}
