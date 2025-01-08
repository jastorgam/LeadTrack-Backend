﻿using LeadTrackApi.Domain.Enums;

namespace LeadTrackApi.Domain.Entities
{
    public class Phone
    {
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public bool Valid { get; set; }
    }
}
