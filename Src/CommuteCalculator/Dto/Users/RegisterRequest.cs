using System.ComponentModel.DataAnnotations;

namespace CommuteCalculator.Dto.Users;

public class RegisterRequest : AuthenticationRequest
{
    [Required]
    public string Name { get; set; } = default!;
}