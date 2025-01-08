using LeadTrackApi.Application.Interfaces;
using LeadTrackApi.Domain.Enums;
using LeadTrackApi.Domain.Models.Request;
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




        [HttpPost]
        [Route("addUser")]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest ur)
        {
            var resp = await _leadBusiness.AddUser(ur.Email, ur.Password, ur.UserName, ur.idRole);
            return Ok(resp);
        }
    }
}
