namespace Infrastructure.Entities;

public class RouteRegistrationEntity
{
    public AddressEntity Origin { get; set; } = default!;
    public AddressEntity Destination { get; set; } = default!;
    public int Distance { get; set; } = default!;
}
