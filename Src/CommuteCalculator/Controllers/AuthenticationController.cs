using AutoMapper;
using CommuteCalculator.Dto.Users;
using CommuteCalculator.Mappers;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CommuteCalculator.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private IAuthenticationService _authenticationService;
    private readonly IMapper _mapper;

    public AuthenticationController(IAuthenticationService authenticationService, IMapper mapper)
    {
        _authenticationService = authenticationService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    [Produces("text/plain")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Login(AuthenticationRequest request)
    {
        var user = _mapper.Map<User>(request);
        var validated = await _authenticationService.LoginAsync(user);
        return validated != null 
            ? Ok(validated) 
            : BadRequest("Username not found or password is incorrect");

    }

    [HttpPost("register")]
    [Produces("text/plain")]
    [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(string))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var userRegistration = _mapper.Map<User>(request);
        var token = await _authenticationService.RegisterAsync(userRegistration);

        return Created(nameof(Register), token);
    }
}