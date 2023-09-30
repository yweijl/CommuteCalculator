using System.Text.Json.Serialization;

namespace Infrastructure.HttpClients.GoogleEntities;

public class Element
{
    [JsonPropertyName("distance")]
    public Distance Distance { get; set; } = default!;

    [JsonPropertyName("duration")]
    public Duration Duration { get; set; } = default!;

    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;
}