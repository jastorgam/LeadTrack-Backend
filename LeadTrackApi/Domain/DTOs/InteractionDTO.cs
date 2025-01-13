using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace LeadTrackApi.Domain.DTOs
{
    public class InteractionDTO
    {
        public string Id { get; set; }
        public string ProspectId { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }
        public bool Answer { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;


    }
}
