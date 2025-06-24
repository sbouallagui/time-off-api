using Swashbuckle.AspNetCore.Filters;
using System.Globalization;
using System;
using Time.Off.Domain.Entities;
using Time.Off.Domain.Enums;
using Time.Off.Domain.ValueObjects;

namespace Time.Off.Api.SwaggerExamples;

public class LeaveRequestExample : IExamplesProvider<LeaveRequest>
{
    public LeaveRequest GetExamples()
    {
        var leaveRequest = new LeaveRequest(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            new LeavePeriod(
                DateOnly.ParseExact("2024-06-23", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                DateOnly.ParseExact("2025-06-28", "yyyy-MM-dd", CultureInfo.InvariantCulture)
            ),
            LeaveType.PaidLeave,
            "Summer vacation"
        );

        leaveRequest.Approve();
        return leaveRequest;
    }

}

