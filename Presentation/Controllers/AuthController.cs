using Application.Contracts;
using Application.DataTransferObjects;
using Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IServiceManager _service;

    public AuthController(IServiceManager service)
    {
        _service = service;
    }

    /// <summary>
    /// Logs in user with email and password
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Jwt Token</returns>
    [HttpPost("login")]
    //[Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(SuccessResponse<AuthDto>), 200)]
    public async Task<IActionResult> Authenticate([FromBody] UserLoginDTO model)
    {
        var response = await _service.AuthenticationService.Login(model);
        return Ok(response);
    }

    /// <summary>
    /// Endpoint to invite a new user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    //[Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(SuccessResponse<UserDTO>), 200)]
    public async Task<IActionResult> RegisterUser([FromBody] UserCreateDTO model)
    {
        var response = await _service.AuthenticationService.RegisterUser(model);
        return Ok(response);
    }
}