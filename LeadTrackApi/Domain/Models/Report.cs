using Org.BouncyCastle.Bcpg.OpenPgp;

namespace LeadTrackApi.Domain.Models;

public class Report
{
    public long TotalProspects { get; set; }
    public long TotalInteractions { get; set; }
    public long TotalInteractionsByPhone { get; set; }
    public long TotalInteractionsByEmail { get; set; }
    public long TotalInteractionsByPhoneTrue { get; set; }
    public long TotalInteractionsByPhoneFalse { get; set; }
    public long TotalInteractionsByEmailTrue { get; set; }
    public long TotalInteractionsByEmailFalse { get; set; }
    public long TotalProspectContacted { get; set; }
    public long TotalProspectContactedTrue { get; set; }

}
