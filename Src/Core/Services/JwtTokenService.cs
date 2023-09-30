using Core.Interfaces;
using Core.Models;
using Core.Models.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtConfig jwtConfig;

    public JwtTokenService(IOptions<JwtConfig> options)
    {
        jwtConfig = options.Value!;
    }

    public string CreateToken(User user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(jwtConfig.AppSecret);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature);
    }

    private static List<Claim> GetClaims(User user)
    {
        return new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString()),
            };
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        return  new JwtSecurityToken(
            issuer: jwtConfig.ValidIssuer,
            audience: jwtConfig.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtConfig.ExpiresInMinutes),
            signingCredentials: signingCredentials);
    }
}