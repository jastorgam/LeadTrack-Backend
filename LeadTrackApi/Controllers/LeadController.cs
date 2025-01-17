using LeadTrackApi.Application.Extensions;
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
    private readonly ILeadBusiness _business = leadBusiness;

    [HttpPost]
    [Route("add-user")]
    public async Task<IActionResult> AddUser([FromBody] AddUserRequest ur)
    {
        var resp = await _business.AddUser(ur.Email, ur.Password, ur.UserName, ur.idRole);
        return Ok(resp);
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
            _logger.LogError(ex, "Error al procesar el archivo.");
            return BadRequest(ex.ToError("Ocurrió un error al procesar el archivo."));
        }
    }



    [HttpGet]
    [Route("get-prospects")]
    public async Task<IActionResult> GetProspects()
    {
        var resp = await _business.GetProspects();
        return Ok(resp);
    }

    [HttpGet]
    [Route("get-interactions/{idProspect}")]
    public async Task<IActionResult> GetInteractions(string idProspect)
    {
        var resp = await _business.GetInteractions(idProspect);
        return Ok(resp);
    }
}
