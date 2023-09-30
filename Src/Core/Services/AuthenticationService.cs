using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthenticationRepository _repository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthenticationService(IAuthenticationRepository repository, IJwtTokenService jwtTokenService, IPasswordHasher<User> passwordHasher)
    {
        _repository = repository;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<string?> LoginAsync(User user)
    {
        var existingUser = await _repository.GetUserByEmailAsync(user.Email);
        if (existingUser == null)
        {
            return null;
        }
        var passwordResult = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password, user.Password);

        if (passwordResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            throw new InvalidOperationException(nameof(passwordResult));
        }

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return GetBearerToken(existingUser);
    }

    public async Task<string> RegisterAsync(User user)
    {
        if (await _repository.UserExistsAsync(user.Email))
        {
            throw new ArgumentException(nameof(user.Email));
        }

        var hashedPassword = _passwordHasher.HashPassword(user, user.Password);
        user.Password = hashedPassword;
        var persistedUser = await _repository.SaveUserAsync(user);

        return GetBearerToken(persistedUser);
    }

    private string GetBearerToken(User user)
        => $"Bearer {_jwtTokenService.CreateToken(user)}";
}