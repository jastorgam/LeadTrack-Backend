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

            if (section.Value != null) configValues[section.Key] = section.Value;
            getChildrens(section, configValues, section.Key);

            return Ok(configValues.Dump());
        }

        private static void getChildrens(IConfigurationSection section, Dictionary<string, string> configValues, string father)
        {
            foreach (var child in section.GetChildren())
            {
                if (child.Value != null) configValues[$"{father}.{child.Key}"] = child.Value;
                if (child.GetChildren().Any()) getChildrens(child, configValues, $"{father}.{child.Key}");

            }
        }
    }
}
