using System.Text.Json.Serialization;

namespace Infrastructure.HttpClients.GoogleEntities;

public class GoogleMapsDistanceEntity
{
    [JsonPropertyName("destination_addresses")]
    public string[] DestinationAddresses { get; set; } = default!;

    [JsonPropertyName("origin_addresses")]
    public string[] OriginAddresses { get; set; } = default!;

    [JsonPropertyName("rows")]
    public Row[] Rows { get; set; } = default!;

    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;
}