using LeadTrackApi.Application.Extensions;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LeadTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController(IConfiguration configuration) : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("GetConfig")]
        public Dictionary<string, string> GetConfig(string seccion)
        {
            var section = configuration.GetSection(seccion);
            var configValues = new Dictionary<string, string>();

            foreach (var child in section.GetChildren())
            {
                configValues[child.Key] = child.Value;
            }

            return configValues;
        }


    }
}
