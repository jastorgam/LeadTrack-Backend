using LeadTrackApi.Application.Extensions;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LeadTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController(IConfiguration configuration) : ControllerBase
    {
        [HttpGet]
        [Route("GetConfig")]
        public async Task<IActionResult> GetConfig(string seccion)
        {
            var key = Request.Headers.TryGetValue("key", out var headerValue);
            if (!key || headerValue != configuration.GetValue<string>("Key"))
                return Unauthorized();

            var section = configuration.GetSection(seccion);
            var configValues = new Dictionary<string, string>();

            foreach (var child in section.GetChildren())
            {
                configValues[child.Key] = child.Value;
            }

            return Ok(configValues.Dump());
        }


    }
}
