using Airways.Domain.EntitiesDB;
using System.Text.Json.Serialization;

public class RapidApiDestinationResponse
{
    [JsonPropertyName("data")]
    public List<RapidApiDestination> RapidApiDestinations { get; set; }

    [JsonPropertyName("status")]
    public bool Status { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}