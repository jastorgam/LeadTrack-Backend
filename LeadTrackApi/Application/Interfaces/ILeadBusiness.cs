using LeadTrackApi.Domain.DTOs;
using LeadTrackApi.Domain.Enums;



namespace LeadTrackApi.Application.Interfaces
{
    public interface ILeadBusiness
    {
        public Task<UserDto> AddUser(string email, string pass, string name, string idRole);
    }
}
