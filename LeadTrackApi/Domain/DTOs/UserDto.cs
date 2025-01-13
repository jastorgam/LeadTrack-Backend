namespace LeadTrackApi.Domain.DTOs
{
    public class UserDto
    {
        public string UserName { get; set; } = null!;
        public string UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
