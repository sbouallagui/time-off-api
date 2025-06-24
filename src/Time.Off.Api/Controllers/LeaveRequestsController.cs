using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection.Metadata;
using Time.Off.Api.SwaggerExamples;
using Time.Off.Application.UseCases.GetLeaveRequest;
using Time.Off.Application.UseCases.LeaveRequestDecision;
using Time.Off.Application.UseCases.SubmitLeaveRequest;
using Time.Off.Domain.Entities;

namespace Time.Off.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LeaveRequestsController(SubmitLeaveRequestHandler submitHandler,
    GetLeaveRequestByIdHandler getHandler,
    LeaveRequestDecisionHandler decisionHandler,
    ILogger<LeaveRequestsController> logger) : ControllerBase
{
    private readonly SubmitLeaveRequestHandler _submitHandler = submitHandler;
    private readonly GetLeaveRequestByIdHandler _getHandler = getHandler;
    private readonly LeaveRequestDecisionHandler _decisionHandler = decisionHandler;
    private readonly ILogger<LeaveRequestsController> _logger = logger;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Submit a leave request",
        Description = "Allows an employee to submit a leave request with specified dates, leave type, and optional comments."
    )]
    [SwaggerResponse(200, "Leave request submitted successfully", typeof(object))]
    [SwaggerResponse(400, "Invalid leave request data or validation error", typeof(object))]
    [SwaggerResponse(500, "An unexpected server error occurred")]
    [SwaggerRequestExample(typeof(RequestLeaveCommand), typeof(RequestLeaveCommandExample))]
    public async Task<IActionResult> RequestLeave([FromBody, SwaggerParameter("The leave request details", Required = true)] RequestLeaveCommand command)
    {
        try
        {
            var result = await _submitHandler.HandleAsync(command);

            if (!result.IsSuccess)
                return BadRequest(new { Error = result.ErrorMessage });

            return Ok(new { RequestId = result.Value });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while submitting a leave request");
            return StatusCode(500, new { Error = "An unexpected error occurred. Please contact support." });
        }
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get leave request by Id",
        Description = "Returns the details of a specific leave request."
    )]
    [SwaggerResponseExample(200, typeof(LeaveRequestExample))]
    [SwaggerResponse(200, "Leave request found", typeof(LeaveRequest))]
    [SwaggerResponse(404, "Leave request not found")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getHandler.HandleAsync(new GetLeaveRequestByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(new { Error = result.ErrorMessage });

        return Ok(result.Value);
    }

    [HttpPut("{leaveRequestId:guid}/review")]
    [SwaggerOperation(
        Summary = "Approve or reject a leave request",
        Description = "Allows an HR manager to review a leave request by approving or rejecting it, optionally adding a comment."
    )]
    [SwaggerResponse(200, "Decision applied successfully")]
    [SwaggerResponse(400, "Validation failed or leave request not found")]
    public async Task<IActionResult> ReviewLeaveRequest( Guid leaveRequestId,
    [FromBody] LeaveRequestDecisionCommand command)
    {
        var result = await _decisionHandler.HandleAsync(leaveRequestId, command);

        if (result.IsSuccess)
            return Ok(new { Message = "Decision applied successfully." });

        return BadRequest(new { Error = result.ErrorMessage });
    }

}
