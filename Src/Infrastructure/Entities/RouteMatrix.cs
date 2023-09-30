using Core.Models;

namespace Infrastructure.Entities;

public class RouteMatrix
{
    public Address Origin { get; set; } = default!;
    public List<Address> Destinations { get; set; } = new();
}