using LeadTrackApi.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly ILogger<LeadController> _logger;
        private readonly ILeadBusiness _leadBusiness;

        public LeadController(ILeadBusiness leadBusiness, ILogger<LeadController> logger)
        {
            _leadBusiness = leadBusiness;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _leadBusiness.GetAllUsers();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(string user, string pass)
        {
            var resp = await _leadBusiness.AddUser(user, pass);
            return Ok(resp);
        }
    }
}
