using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NPOI.OpenXmlFormats.Spreadsheet;

namespace LeadTrackApi.Domain.Entities;

public class FullCompany
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Domain { get; set; }
    public string Size { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
}
