using Moq;
using FluentAssertions;
using Time.Off.Application.UseCases.GetLeaveRequest;
using Time.Off.Domain.Entities;
using Time.Off.Domain.Enums;
using Time.Off.Domain.Repositories;
using Time.Off.Domain.ValueObjects;

namespace Time.Off.UnitTests;

public class GetLeaveRequestByIdHandlerTests
{
    private readonly Mock<ILeaveRequestRepository> _repositoryMock;
    private readonly GetLeaveRequestByIdHandler _handler;

    public GetLeaveRequestByIdHandlerTests()
    {
        _repositoryMock = new Mock<ILeaveRequestRepository>();
        _handler = new GetLeaveRequestByIdHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Success_When_LeaveRequest_Found()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var period = new LeavePeriod(DateOnly.FromDateTime(DateTime.UtcNow),
                                    DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))); 
        var type = LeaveType.PaidLeave;

        var leaveRequest = new LeaveRequest(employeeId, period, type);

        _repositoryMock.Setup(r => r.GetByIdAsync(leaveRequest.Id)).ReturnsAsync(leaveRequest);
        var query = new GetLeaveRequestByIdQuery(leaveRequest.Id);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(leaveRequest);
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Failure_When_LeaveRequest_Not_Found()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(requestId)).ReturnsAsync((LeaveRequest?)null);
        var query = new GetLeaveRequestByIdQuery(requestId);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.ErrorMessage.Should().Be("Leave request not found");
    }
}
