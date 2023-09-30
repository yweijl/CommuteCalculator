namespace Core.Models;

public class Contact
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public Address Address { get; set; } = default!;
    public Guid UserId { get; set; } = default!;
}