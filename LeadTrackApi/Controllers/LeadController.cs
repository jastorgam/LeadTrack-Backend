using LeadTrackApi.Application.Extensions;
using LeadTrackApi.Application.Interfaces;
using LeadTrackApi.Domain.DTOs;
using LeadTrackApi.Domain.Entities;
using LeadTrackApi.Domain.Enums;
using LeadTrackApi.Domain.Models.Request;
using LeadTrackApi.Domain.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadTrackApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeadController(ILeadBusiness leadBusiness, ILogger<LeadController> logger) : ControllerBase
{
    private readonly ILogger<LeadController> _logger = logger;
    private readonly ILeadBusiness _business = leadBusiness;

    [HttpPost]
    [Route("add-user")]
    public async Task<IActionResult> AddUser([FromBody] AddUserRequest ur)
    {
        try
        {
            var resp = await _business.AddUser(ur.Email, ur.Password, ur.UserName, ur.idRole);
            return Ok(resp);
        }
        catch (Exception ex)
        {
            var msj = "Error al agregar usuario";
            _logger.LogError(ex, msj);
            return BadRequest(ex.ToError(msj));
        }
    }

    [HttpPost]
    [Route("verifysavefile")]
    [Authorize]
    public async Task<IActionResult> SaveProspectsFile(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No se ha proporcionado un archivo válido.");
        _logger.LogInformation($"File content {file.ContentType}");
        if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") return BadRequest("El archivo debe ser de tipo excel xlsx.");

        try
        {
            _logger.LogInformation($"Archivo {file.FileName} recibido y procesado correctamente.");

            // Aquí puedes procesar el contenido del archivo con tu lógica de negocio
            var result = await _business.ProcessFile(file.OpenReadStream());

            return Ok(result);
        }
        catch (Exception ex)
        {
            var msj = "Error al procesar archivo";
            _logger.LogError(ex, msj);
            return BadRequest(ex.ToError(msj));
        }
    }

    [HttpPost]
    [Route("verifysavefile-old")]
    [Authorize]
    public async Task<IActionResult> SaveProspectOld(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No se ha proporcionado un archivo válido.");
        _logger.LogInformation($"File content {file.ContentType}");
        if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") return BadRequest("El archivo debe ser de tipo excel xlsx.");

        try
        {
            _logger.LogInformation($"Archivo {file.FileName} recibido y procesado correctamente.");

            // Aquí puedes procesar el contenido del archivo con tu lógica de negocio
            var result = await _business.ProcessFileOld(file.OpenReadStream());

            return Ok(result);
        }
        catch (Exception ex)
        {
            var msj = "Error al procesar archivo";
            _logger.LogError(ex, msj);
            return BadRequest(ex.ToError(msj));
        }
    }



    [HttpGet]
    [Route("listLeads")]
    [Authorize]
    public async Task<IActionResult> GetProspects()
    {
        try
        {
            var resp = await _business.GetFullProspects();
            return Ok(resp);
        }
        catch (Exception ex)
        {
            var msj = "Error al rescatar prospectos";
            _logger.LogError(ex, msj);
            return BadRequest(ex.ToError(msj));
        }
    }

    [HttpGet]
    [Route("{idLead}")]
    [Authorize]
    public async Task<IActionResult> GetProspect(string idLead)
    {
        try
        {
            var resp = await _business.GetFullProspect(idLead);
            return Ok(resp);
        }
        catch (Exception ex)
        {
            var msj = "Error al rescatar prospectos";
            _logger.LogError(ex, msj);
            return BadRequest(ex.ToError(msj));
        }
    }


    [HttpPost]
    [Route("addInteraction")]
    [Authorize]
    public async Task<IActionResult> SaveInteraction([FromBody] InteractionDTO interaction)
    {
        try
        {
            var resp = await _business.SaveInteraction(interaction);
            return Ok(resp);
        }
        catch (Exception ex)
        {
            var msj = "Error al guardar la interacción.";
            _logger.LogError(ex, msj);
            return BadRequest(ex.ToError(msj));
        }

    }

    [HttpGet]
    [Route("getReport")]
    [Authorize]
    public async Task<IActionResult> GetReport()
    {
        try
        {
            var resp = await _business.GetReport();
            return Ok(resp);
        }
        catch (Exception ex)
        {
            var msj = "Error al rescatar reporte";
            _logger.LogError(ex, msj);
            return BadRequest(ex.ToError(msj));
        }

    }



    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateProspect([FromBody] FullProspectDTO fullProspect)
    {
        try
        {
            var resp = await _business.UpdateFullProspect(fullProspect);
            return Ok(resp);
        }
        catch (Exception ex)
        {
            var msj = "Error al guardar la interacción.";
            _logger.LogError(ex, msj);
            return BadRequest(ex.ToError(msj));
        }

    }
}
