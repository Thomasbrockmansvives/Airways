// Models/API/BookingApiModels.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Airways.Models.API
{
    public class HotelSearchResponse
    {
        [JsonPropertyName("primary_count")]
        public int PrimaryCount { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("result")]
        public List<HotelResult> Result { get; set; }
    }

    public class HotelResult
    {
        [JsonPropertyName("hotel_name_trans")]
        public string HotelName { get; set; }

        [JsonPropertyName("review_score_word")]
        public string ReviewScoreWord { get; set; }

        [JsonPropertyName("price_breakdown")]
        public PriceBreakdown PriceBreakdown { get; set; }

        [JsonPropertyName("main_photo_url")]
        public string MainPhotoUrl { get; set; }

        [JsonPropertyName("max_photo_url")]
        public string MaxPhotoUrl { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class PriceBreakdown
    {
        // Store price as a string to preserve the original format
        public string PriceString { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}