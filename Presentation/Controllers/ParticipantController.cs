using API.Controllers.CommonController;
using Application.Contracts;
using Application.DataTransferObjects;
using Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;

namespace Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/participant")]
public class ParticipantController : BaseController
{
    private readonly IServiceManager _service;

    public ParticipantController(IServiceManager service)
    {
        _service = service;
    }


    /// <summary>
    /// Endpoint to invite a new user
    /// </summary>
    /// <param name="meetingCode"></param>
    /// <returns></returns>
    [Authorize(Roles = "User")]
    [HttpPost]
    [Route("join-meeting")]
    [ProducesResponseType(typeof(SuccessResponse<MeetingDTO>), 200)]
    public async Task<IActionResult> JoinMeeting([FromForm] int meetingCode)
    {
        var response = await _service.ParticipantService.JoinMeeting(meetingCode, LoggedInUser);
        return Ok(response);
    }
    
}