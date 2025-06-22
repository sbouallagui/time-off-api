using Dapper;
using Microsoft.Extensions.Logging;
using Time.Off.Domain.Entities;
using Time.Off.Domain.Repositories;
using Time.Off.Infrastructure.Contexts;

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
                leaveRequest.Period.StartDate,
                leaveRequest.Period.EndDate,
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

}
