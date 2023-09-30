namespace Core.Models;

public record Address
{
    public Guid ContactId { get; set; }
    public string PostalCode { get; set; } = default!;
    public string Street { get; set; } = default!;
    public int HouseNumber { get; set; } = default!;
    public string? HouseNumberAddition { get; set; }
    public string City { get; set; } = default!;

    public override string ToString()
    {
        return $"{Street} {HouseNumber} {HouseNumberAddition}, {PostalCode}, {City}";
    }
}