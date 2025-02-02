﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NPOI.OpenXmlFormats.Spreadsheet;

namespace LeadTrackApi.Domain.Entities;

public class Company
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Domain { get; set; }
    public string Size { get; set; }
    public string IdIndustry { get; set; }
    public Industry Industry { get; set; }
}
