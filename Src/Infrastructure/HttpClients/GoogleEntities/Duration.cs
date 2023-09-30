using System.Text.Json.Serialization;

namespace Infrastructure.HttpClients.GoogleEntities;

public class Duration
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = default!;

    [JsonPropertyName("value")]
    public int Value { get; set; } = default!;
}