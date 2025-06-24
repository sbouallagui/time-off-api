using Swashbuckle.AspNetCore.Filters;
using System.Globalization;
using Time.Off.Application.UseCases.SubmitLeaveRequest;
using Time.Off.Domain.Enums;

namespace Time.Off.Api.SwaggerExamples;
public class RequestLeaveCommandExample : IExamplesProvider<RequestLeaveCommand>
{
    public RequestLeaveCommand GetExamples()
    {
        return new RequestLeaveCommand(
            EmployeeId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            StartDate: DateOnly.ParseExact("2025-06-23", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            EndDate: DateOnly.ParseExact("2025-06-27", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Type: LeaveType.SickLeave,
            Comment: "Test leave request"
        );
    }


}
