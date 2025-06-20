using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.off.Domain.Enums;

namespace Time.Off.Application.UseCases.SubmitLeaveRequest
{
    public record RequestLeaveCommand(
        Guid EmployeeId,
        DateTime StartDate,
        DateTime EndDate,
        LeaveType Type,
        string? Comment);
}
