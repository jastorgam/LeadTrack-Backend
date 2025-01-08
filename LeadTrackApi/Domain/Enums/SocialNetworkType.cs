using System.Text.Json.Serialization;

namespace LeadTrackApi.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SocialNetworkType
    {
        Facebook,
        Twitter,
        Instagram,
        LinkedIn,
        Google
    }
}
