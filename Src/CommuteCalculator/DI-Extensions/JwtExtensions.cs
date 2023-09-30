using Core.Models.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CommuteCalculator.DI_Extensions;

public static class JwtExtensions
{
    public static void ConfigureJWT(this WebApplicationBuilder builder)
    {
        var appconfig = builder.Configuration.GetSection(nameof(JwtConfig)).Get<JwtConfig>()!;
        builder.Services.AddAuthentication(
            opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = appconfig.ValidIssuer,
                        ValidAudience = appconfig.ValidAudience,
                        IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appconfig.AppSecret)),
                        ClockSkew = TimeSpan.Zero
                    };
            });
    }
}