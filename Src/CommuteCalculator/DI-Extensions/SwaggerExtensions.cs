using Microsoft.OpenApi.Models;

namespace CommuteCalculator.DI_Extensions;

public static class SwaggerExtensions
{
    public static void AddSwaggerDefintion(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CommuteCalculatorApi",
                Version = "v1",
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Description = "Please enter token",
                Name = "Authorization",
                Scheme = "bearer",
                Type = SecuritySchemeType.Http
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        } });
        });
    }
}