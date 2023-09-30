using AutoMapper;
using CommuteCalculator.Dto.Contacts;
using CommuteCalculator.Dto.Travelplans.Requests;
using CommuteCalculator.Dto.Travelplans.Responses;
using CommuteCalculator.Dto.Users;
using Core.Models;
using Core.Models.Travelplans;

namespace CommuteCalculator.Mappers;

public class ApiMappers : Profile
{
    public ApiMappers()
    {
        //RequestsMappers
        CreateMap<WayPointsRequest, WayPoints>();
        CreateMap<PersistTravelplanRequest, Travelplan>();
        CreateMap<RouteRegistrationRequest, RouteRegistration>();

        CreateMap<AuthenticationRequest, User>();
        CreateMap<RegisterRequest, User>();
        CreateMap<AddContactRequest, Contact>();
        
        //ResponseMappers
        CreateMap<UserTravelplan, UserTravelplanResponse>();
        CreateMap<Travelplan, TravelplanResponse>();
        CreateMap<Contact, ContactResponse>();
        CreateMap<RouteRegistration, RouteRegistrationResponse>();

        // Shared
        CreateMap<NavigationAddress, Address>().ReverseMap();

    }
}
