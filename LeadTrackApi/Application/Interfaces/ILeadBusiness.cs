using LeadTrackApi.Domain.DTOs;

namespace LeadTrackApi.Application.Interfaces;

public interface ILeadBusiness
{
    public Task<UserDto> AddUser(string email, string pass, string name, string idRole);
    public Task<List<ProspectDTO>> GetProspects();
    public Task<List<InteractionDTO>> GetInteractions(string idProspect);
    public Task<List<Dictionary<string, object>>> ProcessFile(Stream stream);
}
