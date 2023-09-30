namespace Infrastructure.Entities;

public class ContactEntity : EntityBase
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public AddressEntity Address { get; set; } = default!;
    public Guid UserId { get; set; } = default!;
}