using Core.Models;

namespace Core.Interfaces;

public interface IJwtTokenService
{
    string CreateToken(User user);
}