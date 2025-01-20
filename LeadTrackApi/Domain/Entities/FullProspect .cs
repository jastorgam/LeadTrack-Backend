using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LeadTrackApi.Domain.Entities;

public class FullProspect
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string Position { get; set; }
    public List<Phone> Phones { get; set; }
    public List<Email> Emails { get; set; }
    public List<SocialNetwork> SocialNetworks { get; set; }
    public FullCompany Company { get; set; }
    public List<FullInteractions> Interactions { get; set; }
    public bool Status { get; set; }
    public DateTime? DateModify { get; set; } = null;
    public string UserModify { get; set; }
}
