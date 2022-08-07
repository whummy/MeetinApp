using Application.Contracts;
using Application.DataTransferObjects;
using Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/meeting")]
public class MeetingController : ControllerBase
{
    private readonly IServiceManager _service;

    public MeetingController(IServiceManager service)
    {
        _service = service;
    }


    /// <summary>
    /// Endpoint to invite a new user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Roles = "User")]
    [HttpPost]
    [Route("create-meeting")]
    [ProducesResponseType(typeof(SuccessResponse<MeetingDTO>), 200)]
    public async Task<IActionResult> CreateMeeting([FromBody] MeetingCreateDTO model)
    {
        var response = await _service.MeetingService.CreateMeeting(model);
        return Ok(response);
    }
}