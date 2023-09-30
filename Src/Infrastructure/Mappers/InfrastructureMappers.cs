using AutoMapper;
using Core.Models;
using Core.Models.Travelplans;
using Infrastructure.Entities;

namespace Infrastructure.Mappers;

public class InfrastructureMappers : Profile
{
    public InfrastructureMappers()
    {
        CreateMap<Contact, ContactEntity>().ReverseMap();
        CreateMap<User, UserEntity>()
            .ForMember(x => x.PasswordHash, opt => opt.MapFrom(x => x.Password))
            .ReverseMap();
        CreateMap<CalculatedRoutes, CalculatedRoutesEntity>().ReverseMap();
        CreateMap<Destinations, DestinationEntity>().ReverseMap();
        CreateMap<UserTravelplan, UserTravelplanEntity>().ReverseMap();
        CreateMap<Travelplan, TravelplanEntity>().ReverseMap();
        CreateMap<Address, AddressEntity>().ReverseMap();
        CreateMap<RouteRegistration, RouteRegistrationEntity>().ReverseMap();
    }
}
