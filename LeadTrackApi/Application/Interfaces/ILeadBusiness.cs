using LeadTrackApi.Domain.Models;

namespace LeadTrackApi.Application.Interfaces
{
    public interface ILeadBusiness
    {
        public Task<List<User>> GetAllUsers();
        public Task<User> AddUser(string user, string pass);
    }
}
