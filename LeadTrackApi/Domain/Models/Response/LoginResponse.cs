namespace LeadTrackApi.Domain.Models.Response;

public class LoginResponse
{
    public string Token { get; set; }
    public string Role { get; set; }
    public string UserName { get; set; }
}
