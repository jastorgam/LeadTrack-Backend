using LeadTrackApi.Application.Interfaces;
using LeadTrackApi.Domain.Models.Request;
using LeadTrackApi.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace LeadTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthBusiness authBusiness) : ControllerBase
    {
        private readonly IAuthBusiness _authBusiness = authBusiness;

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null) return BadRequest(new { Error = "Invalid client request" });

            try
            {
                var resp = await _authBusiness.Login(loginRequest.Email, loginRequest.Password);
                return Ok(new LoginResponse() { UserName = resp.UserName, Token = resp.Token, Role = resp.Role });

            }
            catch (Exception e)
            {
                return BadRequest(new { Error = e.Message });
            }
        }
    }
}
