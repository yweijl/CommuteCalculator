using Core.Models;

namespace CommuteCalculator.Dto.Contacts;

public class ContactResponse
{
    public Guid Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public Address Address { get; set; } = default!;
}