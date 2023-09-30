using System.ComponentModel.DataAnnotations;

namespace CommuteCalculator.Dto.Contacts;

public class AddContactRequest
{
    [Required]
    public string FirstName { get; set; } = default!;
    [Required]
    public string LastName { get; set; } = default!;
    [Required]
    [RegularExpression(@"^[1-9][0-9]{3}\s*(?:[a-zA-Z]{2})?$", ErrorMessage = "Postalcode should be in format 1234AB")]
    public string PostalCode { get; set; } = default!;
    [Required]
    public string Street { get; set; } = default!;
    [Required]
    public int HouseNumber { get; set; } = default!;
    public string? HouseNumberAddition { get; set; } = default!;
    [Required]
    public string City { get; set; } = default!;
}