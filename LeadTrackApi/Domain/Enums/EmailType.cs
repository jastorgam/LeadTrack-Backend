using System.Text.Json.Serialization;

namespace LeadTrackApi.Domain.Enums
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EmailType
    {
        Personal,
        Work,
        Other
    }

}
