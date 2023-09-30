namespace Infrastructure.Entities;

public class AddressEntity
{
    public string PostalCode { get; set; } = default!;
    public string Street { get; set; } = default!;
    public int HouseNumber { get; set; } = default!;
    public string? HouseNumberAddition { get; set; } = default!;
    public string City { get; set; } = default!;
    public Guid ContactId { get; set; }
}