using LeadTrackApi.Application.Interfaces;
using LeadTrackApi.Application.Services;
using LeadTrackApi.Application.Utils;
using LeadTrackApi.Domain.Models.Response;
using LeadTrackApi.Services;

namespace LeadTrackApi.Application.Business
{
    public class AuthBusiness(JwtService jwtService, MongoDBService mongoDBService) : IAuthBusiness
    {
        private readonly JwtService _jwtService = jwtService;
        private readonly MongoDBService _mongoDBService = mongoDBService;

        public async Task<LoginResponse> Login(string email, string password)
        {
            password = SecurityUtils.HashPassword(password);

            var resp = await _mongoDBService.Login(email, password);
            if (resp == null) throw new Exception("Invalid credentials");

            return new LoginResponse()
            {
                UserName = resp.Username,
                Token = _jwtService.GenerateToken(resp.Email, resp.Role),
                Role = resp.Role
            };

        }
    }
}
