using LeadTrackApi.Application.Interfaces;
using LeadTrackApi.Application.Utils;
using LeadTrackApi.Domain.DTOs;
using LeadTrackApi.Domain.Entities;
using LeadTrackApi.Persistence.Service;

namespace LeadTrackApi.Application.Business
{
    public class LeadBusiness(MongoDBService mongoDBService) : ILeadBusiness
    {
        private readonly MongoDBService _mongoService = mongoDBService;

        public async Task<UserDto> AddUser(string email, string pass, string name, string idRole)
        {
            pass = SecurityUtils.HashPassword(pass);
            return await _mongoService.AddUser(email, pass, name, idRole);
        }

        public async Task<List<ProspectDTO>> GetProspects()
        {
            return await _mongoService.GetProspects();
        }

        public async Task<List<InteractionDTO>> GetInteractions(string idProspect)
        {
            return await _mongoService.GetInteractionsByProspect(idProspect);
        }
    }
}
