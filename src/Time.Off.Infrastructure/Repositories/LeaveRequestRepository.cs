using Dapper;
using Microsoft.Extensions.Logging;
using Time.Off.Domain.Entities;
using Time.Off.Domain.Repositories;
using Time.Off.Domain.ValueObjects;
using Time.Off.Infrastructure.Contexts;
using Time.Off.Infrastructure.Dtos;

namespace Time.Off.Infrastructure.Repositories;

public class LeaveRequestRepository(TimeOffDataBaseContext context, ILogger<LeaveRequestRepository> logger) : ILeaveRequestRepository
{
    private readonly TimeOffDataBaseContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ILogger<LeaveRequestRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Guid> AddAsync(LeaveRequest leaveRequest)
    {
        try
        {
            string query = Queries.InsertIntoLeaveRequest;

            using var connection = _context.CreateConnection();
            await connection.OpenAsync();

            var parameters = new
            {
                leaveRequest.Id,
                leaveRequest.EmployeeId,
                StartDate = leaveRequest.Period?.StartDate.ToDateTime(TimeOnly.MinValue),
                EndDate = leaveRequest.Period?.EndDate.ToDateTime(TimeOnly.MinValue),
                Type = leaveRequest.Type.ToString(),
                leaveRequest.Comment,
                Status = leaveRequest.Status.ToString(),
                leaveRequest.CreatedAt,
                ModifiedAt = leaveRequest.CreatedAt
            };

            // Execute the query and get the returned Id
            var insertedId = await connection.ExecuteScalarAsync<Guid>(query, parameters);

            return insertedId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in LeaveRequestRepository.AddAsync");
            throw;
        }
    }

    public async Task<LeaveRequest?> GetByIdAsync(Guid id)
    {
        try
        {
            string query = Queries.GetLeaveRequestById;

            using var connection = _context.CreateConnection();
            await connection.OpenAsync();

            var leaveRequests = await connection.QueryAsync<LeaveRequestDto, LeavePeriodDto, LeaveRequest>(
                query,
                (dto, periodDto) =>
                {
                    // Convert DateTime to DateOnly here
                    var period = new LeavePeriod(
                    DateOnly.FromDateTime(periodDto.StartDate),
                    DateOnly.FromDateTime(periodDto.EndDate));

                    var lr = new LeaveRequest(dto.EmployeeId, period, dto.Type, dto.Comment);

                    var type = typeof(LeaveRequest);
                    type.GetProperty("Id")?.SetValue(lr, dto.Id);
                    type.GetProperty("Status")?.SetValue(lr, dto.Status);
                    type.GetProperty("ModifiedAt")?.SetValue(lr, dto.ModifiedAt);

                    return lr;
                },
                param: new { Id = id },
                splitOn: "StartDate"
            );

            return leaveRequests.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching LeaveRequest with Id {LeaveRequestId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsPendingRequestForPeriodAsync(Guid employeeId, LeavePeriod period)
    {
        string query = Queries.ExistsPendingRequestForPeriodQuery;


        using var connection = _context.CreateConnection();
        await connection.OpenAsync();

        var result = await connection.ExecuteScalarAsync<int?>(
            query,
            new
            {
                EmployeeId = employeeId,
                StartDate = period.StartDate.ToDateTime(TimeOnly.MinValue),
                EndDate = period.EndDate.ToDateTime(TimeOnly.MinValue)
            });

        return result.HasValue;
    }

    public async Task UpdateAsync(LeaveRequest leaveRequest)
    {
        string query = Queries.UpdateLeaveRequestStatus;

        await using var connection = _context.CreateConnection();
        await connection.OpenAsync();

        var parameters = new
        {
            Id = leaveRequest.Id,
            Status = leaveRequest.Status.ToString(),
            ManagerComment = leaveRequest.ManagerComment,
            ModifiedAt = DateTime.UtcNow
        };

        await connection.ExecuteAsync(query, parameters);
    }



}
