using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Time.Off.Application.UseCases.SubmitLeaveRequest;

namespace Time.Off.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveRequestsController(SubmitLeaveRequestHandler submitHandler) : ControllerBase
{
    private readonly SubmitLeaveRequestHandler _submitHandler = submitHandler;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Submit a leave request",
        Description = "Allows an employee to submit a leave request with specified dates, leave type, and optional comments."
    )]
    [SwaggerResponse(200, "Leave request submitted successfully", typeof(object))]
    [SwaggerResponse(400, "Invalid leave request data or validation error", typeof(object))]
    public async Task<IActionResult> RequestLeave([FromBody, SwaggerParameter("The leave request details", Required = true)] RequestLeaveCommand command)
    {
        var result = await _submitHandler.HandleAsync(command);

        if (!result.IsSuccess)
            return BadRequest(new { Error = result.ErrorMessage});

        return Ok(new { RequestId = result.Value });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        //var result = await _getHandler.HandleAsync(id);

        //if (!result.IsSuccess)
        //    return NotFound(new { Error = result.ErrorMessage });

        return Ok();
    }
}
