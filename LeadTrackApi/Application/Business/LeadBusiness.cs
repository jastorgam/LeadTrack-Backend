using LeadTrackApi.Application.Interfaces;
using LeadTrackApi.Domain.Models;
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

        public async Task<List<User>> GetAllUsers()
        {
            return await _mongoService.GetAsync();
        }

        public async Task<User> AddUser(string user, string pass)
        {
            return await _mongoService.AddUser(user, pass);
        }
    }
}
