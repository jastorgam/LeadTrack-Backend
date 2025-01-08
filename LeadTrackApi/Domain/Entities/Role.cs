using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LeadTrackApi.Domain.Entities
{
    public class Role
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
