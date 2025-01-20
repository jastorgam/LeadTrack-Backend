using LeadTrackApi.Application.Extensions;
using LeadTrackApi.Application.Interfaces;
using LeadTrackApi.Domain.DTOs;
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
    [Route("load-file-prospect")]
    public async Task<IActionResult> SaveProspect(IFormFile file)
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



    [HttpGet]
    [Route("prospects")]
    public async Task<IActionResult> GetProspects([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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
    [Route("prospects/count")]
    public async Task<IActionResult> GetProspectsCount([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var resp = await _business.GetProspectsCount();
            return Ok(resp);
        }
        catch (Exception ex)
        {
            var msj = "Error al contar prospectos";
            _logger.LogError(ex, msj);
            return BadRequest(ex.ToError(msj));
        }
    }

    [HttpGet]
    [Route("interaction/get/{idProspect}")]
    public async Task<IActionResult> GetInteractions(string idProspect)
    {
        try
        {
            var resp = await _business.GetInteractions(idProspect);
            return Ok(resp);
        }
        catch (Exception ex)
        {
            var msj = "Error al rescatar interacciones";
            _logger.LogError(ex, msj);
            return BadRequest(ex.ToError(msj));
        }
    }


    [HttpPost]
    [Route("interaction/save")]
    public async Task<IActionResult> SaveInteraction([FromBody] InteractionDTO interaction)
    {
        try
        {
            await _business.SaveInteraction(interaction);
            return Ok();
        }
        catch (Exception ex)
        {
            var msj = "Error al guardar la interacción.";
            _logger.LogError(ex, msj);
            return BadRequest(ex.ToError(msj));
        }

    }
}
