using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using LeadTrackApi.Domain.Enums;
using System.Text.Json.Serialization;

namespace LeadTrackApi.Domain.Entities
{
    public class Interacctions
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ProspectId { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }
        public bool Answer { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;


    }
}
