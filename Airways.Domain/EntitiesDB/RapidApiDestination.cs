using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Airways.Domain.EntitiesDB
{
    public class RapidApiDestination
    {
        [JsonPropertyName("dest_id")] 
        public string DestId { get; set; }

        [JsonPropertyName("name")] 
        public string CityName { get; set; }
    }
}