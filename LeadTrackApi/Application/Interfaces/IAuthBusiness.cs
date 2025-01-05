using LeadTrackApi.Application.Services;
using LeadTrackApi.Domain.Models.Response;

namespace LeadTrackApi.Application.Interfaces
{
    public interface IAuthBusiness
    {
        public Task<LoginResponse> Login(string email, string password);
    }
}
