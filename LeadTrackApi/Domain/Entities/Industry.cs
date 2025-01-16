using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LeadTrackApi.Domain.Entities;

public class Industry
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
}
