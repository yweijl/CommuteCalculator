using System.Text.Json.Serialization;

namespace Infrastructure.HttpClients.GoogleEntities;

public class Row
{
    [JsonPropertyName("elements")]
    public List<Element> Elements { get; set; } = default!;
}