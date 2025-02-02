﻿using LeadTrackApi.Application.Extensions;
using LeadTrackApi.Application.Interfaces;
using LeadTrackApi.Domain.Models.Request;
using LeadTrackApi.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace LeadTrackApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthBusiness authBusiness, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IAuthBusiness _authBusiness = authBusiness;

    /// <summary>
    /// Metodo de login para obtener token de autenticacion
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            var resp = await _authBusiness.Login(loginRequest.Email, loginRequest.Password);
            return Ok(new LoginResponse() { UserName = resp.UserName, Token = resp.Token, Role = resp.Role });

        }
        catch (Exception e)
        {
            logger.LogError(message: e.ToError().ToString());
            return Unauthorized();
        }
    }

    //[HttpGet]
    //[Route("hello")]
    //public async Task<IActionResult> Hello()
    //{
    //    try
    //    {

    //        return Ok(new { msg = "Hello" });

    //    }
    //    catch (Exception e)
    //    {
    //        logger.LogError(message: e.ToError().ToString());
    //        return Unauthorized();
    //    }
    //}
}
