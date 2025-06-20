using Microsoft.AspNetCore.Mvc;
using Time.Off.Application.UseCases.SubmitLeaveRequest;

[ApiController]
[Route("api/[controller]")]
public class LeaveRequestsController(SubmitLeaveRequestHandler handler) : ControllerBase
{
    private readonly SubmitLeaveRequestHandler _handler = handler;

    [HttpPost]
    public async Task<IActionResult> RequestLeave([FromBody] RequestLeaveCommand command)
    {
        var result = await _handler.HandleAsync(command);

        if (!result.IsSuccess)
            return BadRequest(new { Error = result.ErrorMessage});

        return Ok(new { RequestId = result.Value });
    }
}
