using Core.Models;

namespace Core.Interfaces;

public interface IAuthenticationService
{
    Task<string?> LoginAsync(User user);
    Task<string> RegisterAsync(User user);
}