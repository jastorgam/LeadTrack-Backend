using System.Text.Json.Serialization;

namespace LeadTrackApi.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InteractionType
{
    Call,
    Email,
    Meeting
}
