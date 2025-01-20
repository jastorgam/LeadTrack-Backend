namespace LeadTrackApi.Domain.Entities;

public class FullInteractions
{
    public string UserName { get; set; }
    public string Type { get; set; }
    public string Notes { get; set; }
    public bool Answer { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
}
