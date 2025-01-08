using LeadTrackApi.Application.Interfaces;
using LeadTrackApi.Application.Utils;
using LeadTrackApi.Domain.DTOs;
using LeadTrackApi.Domain.Enums;
using LeadTrackApi.Services;

namespace LeadTrackApi.Application.Business
{
    public class LeadBusiness : ILeadBusiness
    {
        private readonly MongoDBService _mongoService;

        public LeadBusiness(MongoDBService mongoDBService)
        {
            _mongoService = mongoDBService;
        }


        public async Task<UserDto> AddUser(string email, string pass, string name, string idRole)
        {
            pass = SecurityUtils.HashPassword(pass);
            return await _mongoService.AddUser(email, pass, name, idRole);
        }
    }
}
