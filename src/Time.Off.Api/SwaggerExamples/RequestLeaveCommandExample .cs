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
            StartDate: DateTime.Parse("2025-06-23T15:08:07.858Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
            EndDate: DateTime.Parse("2025-06-27T15:08:07.858Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
            Type: LeaveType.SickLeave,
            Comment: "Test leave request"
        );
    }
}
