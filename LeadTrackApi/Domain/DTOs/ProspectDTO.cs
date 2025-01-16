using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using LeadTrackApi.Domain.Entities;

namespace LeadTrackApi.Domain.DTOs;

public class ProspectDTO
{

    public string Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Position { get; set; }
    public List<Phone> Phones { get; set; }
    public List<Email> Emails { get; set; }
    public List<SocialNetwork> SocialNetworks { get; set; }
    public CompanyDTO Company { get; set; }
}
