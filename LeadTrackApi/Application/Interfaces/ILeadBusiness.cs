using LeadTrackApi.Domain.DTOs;
using LeadTrackApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LeadTrackApi.Application.Interfaces;

public interface ILeadBusiness
{
    public Task<UserDto> AddUser(string email, string pass, string name, string idRole);
    public Task<List<ProspectDTO>> GetProspects(int page = 1, int pageSize = 10);
    public Task<IEnumerable<FullProspect>> GetFullProspects();
    public Task<long> GetProspectsCount();
    public Task<List<InteractionDTO>> GetInteractions(string idProspect);
    public Task<List<Dictionary<string, object>>> ProcessFile(Stream stream);
    public Task<FullProspect> SaveInteraction(InteractionDTO interaction);
    public Task<FullProspect> GetFullProspect(string id);
}
