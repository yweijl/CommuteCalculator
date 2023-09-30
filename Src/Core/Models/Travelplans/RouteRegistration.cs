namespace Core.Models.Travelplans;

public class RouteRegistration
{
    public Address Origin { get; set; } = default!;
    public Address Destination { get; set; } = default!;
    public int Distance { get; set; } = default!;
}