namespace Infrastructure.CosmosDb;

public class CosmosDbSettings
{
    public string EndPoint { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string Database { get; set; } = default!;
}