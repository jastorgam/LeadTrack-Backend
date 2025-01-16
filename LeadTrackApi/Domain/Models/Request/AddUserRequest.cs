using LeadTrackApi.Domain.Enums;

namespace LeadTrackApi.Domain.Models.Request;

public class AddUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public string idRole { get; set; }
}
