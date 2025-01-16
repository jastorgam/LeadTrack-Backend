using LeadTrackApi.Application.Interfaces;
using LeadTrackApi.Domain.Enums;
using LeadTrackApi.Domain.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadTrackApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeadController(ILeadBusiness leadBusiness, ILogger<LeadController> logger) : ControllerBase
{
    private readonly ILogger<LeadController> _logger = logger;
    private readonly ILeadBusiness _leadBusiness = leadBusiness;

    [HttpPost]
    [Route("add-user")]
    public async Task<IActionResult> AddUser([FromBody] AddUserRequest ur)
    {
        var resp = await _leadBusiness.AddUser(ur.Email, ur.Password, ur.UserName, ur.idRole);
        return Ok(resp);
    }

    [HttpGet]
    [Route("get-prospects")]
    public async Task<IActionResult> GetProspects([FromBody] AddUserRequest ur)
    {
        var resp = await _leadBusiness.GetProspects();
        return Ok(resp);
    }

    [HttpGet]
    [Route("get-interactions/{idProspect}")]
    public async Task<IActionResult> GetInteractions(string idProspect)
    {
        var resp = await _leadBusiness.GetInteractions(idProspect);
        return Ok(resp);
    }
}
