namespace LeadTrackApi.Domain.Models.Response
{
    public class ErrorResponse
    {
        public string? Error { get; set; }
        public string? Stack { get; set; }
        public string? Inner { get; set; }
    }
}
