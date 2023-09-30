namespace CommuteCalculator.Dto.Users;

using System.ComponentModel.DataAnnotations;

public class AuthenticationRequest
{
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}