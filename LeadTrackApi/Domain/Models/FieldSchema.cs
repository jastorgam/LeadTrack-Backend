namespace LeadTrackApi.Domain.Models;

public class FieldSchema
{
    public string Name { get; set; }
    public string Type { get; set; }
    public int Length { get; set; } // Para archivos de largo fijo
    public string Format { get; set; } // Para fechas u otros tipos con formato
}
