using AutoMapper;
using Core.Models;
using Infrastructure.CosmosDb;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CommuteCalculator.DI_Extensions;

public static class DatabaseExtentions
{
    public static void SeedDatabase(this IServiceProvider serviceProvider)
    {
        var serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>()!;
        using var scope = serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<CommuteCalculatorContext>()!;
        var hasher = scope.ServiceProvider.GetService<IPasswordHasher<User>>()!;
        var mapper = scope.ServiceProvider.GetService<IMapper>();
        
        if (context.Database.EnsureCreated())
        {
            var userobj = new User
            {
                Name = "Anna",
                Email = "anna@gmail.com",
            };

            userobj.Password = hasher.HashPassword(userobj, "123456ab");


            context.Users.Add(mapper!.Map<UserEntity>(userobj));

            context.SaveChanges();
            var user = context.Users.SingleOrDefault(x => x.Name == "Anna");
            context.Contacts.AddRange(new ContactEntity
            {
                FirstName = "Dhr Jh",
                LastName = "van kant",
                UserId = user!.Id,
                Address = new AddressEntity
                {
                    City = "Den Haag",
                    HouseNumber = 12,
                    HouseNumberAddition = "",
                    PostalCode = "2598 aV",
                    Street = "kampStraat"
                }
            },
            new ContactEntity
            {
                FirstName = "Sam",
                LastName = "Nam",
                UserId = user!.Id,
                Address = new AddressEntity
                {
                    City = "Den Haag",
                    HouseNumber = 65,
                    HouseNumberAddition = "A",
                    PostalCode = "2229 RB",
                    Street = "Breugstraat"
                }
            },
            new ContactEntity
            {
                FirstName = "Saf",
                LastName = "Korv",
                UserId = user!.Id,
                Address = new AddressEntity
                {
                    City = "Den Haag",
                    HouseNumber = 355,
                    PostalCode = "2522 SS",
                    Street = "Roggestraat"
                }
            },
            new ContactEntity
            {
                FirstName = "David",
                LastName = "Oesters",
                UserId = user!.Id,
                Address = new AddressEntity
                {
                    City = "Den Haag",
                    HouseNumber = 35,
                    PostalCode = "2296 ZB",
                    Street = "schiplaan"
                }
            },
            new ContactEntity
            {
                FirstName = "Sabri",
                LastName = "Zoubaa",
                UserId = user!.Id,
                Address = new AddressEntity
                {
                    City = "Den Haag",
                    HouseNumber = 28,
                    PostalCode = "2545 WL",
                    Street = "Wachtumstraat"
                }
            },
            new ContactEntity
            {
                FirstName = "Cas",
                LastName = "Beek",
                UserId = user!.Id,
                Address = new AddressEntity
                {
                    City = "Den Haag",
                    HouseNumber = 17,
                    PostalCode = "2595 VG",
                    Street = "Klimopstraat"
                }
            },
            new ContactEntity
            {
                FirstName = "Arda",
                LastName = "Laan",
                UserId = user!.Id,
                Address = new AddressEntity
                {
                    City = "Den Haag",
                    HouseNumber = 89,
                    PostalCode = "2525 VH",
                    Street = "vermeerstraat"
                }
            },
            new ContactEntity
            {
                FirstName = "College",
                LastName = "St Paul",
                UserId = user!.Id,
                Address = new AddressEntity
                {
                    City = "Den Haag",
                    HouseNumber = 1,
                    PostalCode = "2533 AG",
                    Street = "llaland 119"
                }
            },
            new ContactEntity
            {
                FirstName = "Ivio",
                LastName = "school",
                UserId = user!.Id,
                Address = new AddressEntity
                {
                    City = "Den Haag",
                    HouseNumber = 91,
                    PostalCode = "2566 EA",
                    Street = "Laan van poot"
                }
            },
            new ContactEntity
            {
                FirstName = "An",
                LastName = "bokon",
                UserId = user!.Id,
                Address = new AddressEntity
                {
                    City = "Den Haag",
                    HouseNumber = 65,
                    PostalCode = "2533GP",
                    Street = "De kade"
                }
            }
            );

            context.SaveChanges();
        }
    }

    public static void ConfigureCosmosDb(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<CosmosDbSettings>(builder.Configuration.GetSection(nameof(CosmosDbSettings)));
        builder.Services.AddDbContextFactory<CommuteCalculatorContext>(
            (serviceProvider, options) =>
            {
                var cosmosSettings = serviceProvider.GetRequiredService<IOptions<CosmosDbSettings>>().Value;
                options.UseCosmos(cosmosSettings.EndPoint, cosmosSettings.Key, cosmosSettings.Database);
            });
    }
}
