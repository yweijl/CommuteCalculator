namespace Core.Models.Travelplans;

public class CalculatedRoutes
{
    public Guid Id { get; set; }
    public Address Origin { get; set; } = default!;
    public List<Destinations> Destinations { get; set; } = default!;
}

