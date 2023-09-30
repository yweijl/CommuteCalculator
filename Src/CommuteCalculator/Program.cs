using CommuteCalculator.DI_Extensions;
using CommuteCalculator.Mappers;
using Core.Interfaces;
using Core.Models;
using Core.Models.Configuration;
using Core.Services;
using Infrastructure.HttpClients;
using Infrastructure.Mappers;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

const string allowedOrigins = "AllowedOrigins";
var origins = builder.Configuration[allowedOrigins];
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins, builder =>
    {
        builder.WithOrigins(origins)
        .AllowAnyMethod()
        .WithHeaders(new string[] { HeaderNames.ContentType, HeaderNames.Authorization })
        .WithExposedHeaders("Content-Disposition");
    });
});

builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddEndpointsApiExplorer();


// Configure DI-Extentions
builder.Services.AddSwaggerDefintion();
builder.ConfigureCosmosDb();
builder.ConfigureJWT();

builder.Services
    .AddAutoMapper(
        Assembly.GetAssembly(typeof(ApiMappers)), 
        Assembly.GetAssembly(typeof(InfrastructureMappers)));

builder.Services.Configure<GoogleMapsConfig>(builder.Configuration.GetSection(nameof(GoogleMapsConfig)));
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection(nameof(JwtConfig)));

builder.Services.AddHttpClient<IGoogleMapsClient, GoogleMapsClient>();

builder.Services.AddScoped<IUserTravelplanService, UserTravelplanService>();
builder.Services.AddScoped<IUserTravelplansRepository, UserTravelplansRepository>();
builder.Services.AddScoped<ICalculatedRoutesRepository, CalculatedRoutesRepository>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ITravelplanService, TravelplanService>();
builder.Services.AddScoped<ICsvService, CsvService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.Services.SeedDatabase();
}

app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors(allowedOrigins);


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();