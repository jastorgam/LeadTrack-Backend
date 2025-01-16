using LeadTrackApi.Domain.Enums;

namespace LeadTrackApi.Domain.Entities;

public class Email
{
    public string Address { get; set; }
    public string Type { get; set; }
    public bool Valid { get; set; }
}
